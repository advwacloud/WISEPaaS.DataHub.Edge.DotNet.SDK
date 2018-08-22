﻿using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Implementations;
using MQTTnet.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WISEPaaS.SCADA.DotNet.SDK.Model;

namespace WISEPaaS.SCADA.DotNet.SDK
{
    public class EdgeAgent
    {
        public const int DEAFAULT_HEARTBEAT_INTERVAL = 60000;
        public const int DEAFAULT_DATARECOVER_INTERVAL = 3000;
        public const int DEAFAULT_DATARECOVER_COUNT = 1;

        private ManagedMqttClient _mqttClient;
        private DataRecoverHelper _recoverHelper;
        
        //private static Logger _logger = LogManager.GetCurrentClassLogger();

        private string _configTopic;
        private string _dataTopic;
        private string _scadaConnTopic;
        private string _deviceConnTopic;
        private string _cmdTopic;
        private string _ackTopic;
        private string _cfgAckTopic;

        private Timer _heartbeatTimer;
        private Timer _dataRecoverTimer;

        private EdgeAgentOptions _options;
        public EdgeAgentOptions Options
        {
            get { return _options; }
            set { _options = value; }
        }

        public bool IsConnected
        {
            get { return _mqttClient.IsConnected; }
        }

        public event EventHandler<EdgeAgentConnectedEventArgs> Connected;
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public EdgeAgent( EdgeAgentOptions options )
        {
            Options = options;
            _mqttClient = new MqttFactory().CreateManagedMqttClient() as ManagedMqttClient;

            _mqttClient.ApplicationMessageReceived += mqttClient_MessageReceived;
            _mqttClient.Connected += mqttClient_Connected;
            _mqttClient.Disconnected += mqttClient_Disconnected;

            _heartbeatTimer = new Timer();
            _heartbeatTimer.Interval = _options.Heartbeat;
            _heartbeatTimer.Elapsed += _heartbeatTimer_Elapsed;

            if ( options.DataRecover )
            {
                _recoverHelper = new DataRecoverHelper();
                _dataRecoverTimer = new Timer();
                _dataRecoverTimer.Interval = DEAFAULT_DATARECOVER_INTERVAL;
                _dataRecoverTimer.Elapsed += _dataRecoverTimer_Elapsed;
                _dataRecoverTimer.Enabled = true;
            }

            MqttTcpChannel.CustomCertificateValidationCallback = ( x509Certificate, x509Chain, sslPolicyErrors, mqttClientTcpOptions ) => { return true; };
        }

        #region >>> Private Method <<<

        // for self-signed cert
        private bool checkValidationResult( object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors )
        { 
            return true;
        }

        private void _dataRecoverTimer_Elapsed( object sender, ElapsedEventArgs e )
        {
            if ( _mqttClient.IsConnected == false )
                return;

            if ( _recoverHelper != null && _recoverHelper.DataAvailable() )
            {
                List<string> records = _recoverHelper.Read( DEAFAULT_DATARECOVER_COUNT );
                foreach ( var record in records )
                {
                    var message = new MqttApplicationMessageBuilder()
                    .WithTopic( _dataTopic )
                    .WithPayload( record )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( false )
                    .Build();

                    _mqttClient.PublishAsync( message );
                }
            }
        }

        private void _heartbeatTimer_Elapsed( object sender, ElapsedEventArgs e )
        {
            HeartbeatMessage heartbeatMsg = new HeartbeatMessage();
            string payload = JsonConvert.SerializeObject( heartbeatMsg );

            var message = new MqttApplicationMessageBuilder()
            .WithTopic( ( _options.Type == EdgeType.Gatway ) ? _scadaConnTopic : _deviceConnTopic )
            .WithPayload( payload )
            .WithAtLeastOnceQoS()
            .WithRetainFlag( true )
            .Build();

            _mqttClient.PublishAsync( message );
        }

        private bool _getCredentialFromDCCS()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback( checkValidationResult );

                using ( WebClient client = new WebClient() )
                {
                    Uri uri = new Uri( new Uri( Options.DCCS.APIUrl ), "v1/serviceCredentials/" + Options.DCCS.CredentialKey );
                    string response = client.DownloadString( uri );
                    dynamic json = JObject.Parse( response );
                    dynamic credential = json.credential;

                    Options.MQTT.HostName = json.serviceHost;
                    if ( Options.UseSecure )
                    {
                        Options.MQTT.Port = credential.protocols["mqtt+ssl"].port;
                        Options.MQTT.Username = credential.protocols["mqtt+ssl"].username;
                        Options.MQTT.Password = credential.protocols["mqtt+ssl"].password;
                    }
                    else
                    {
                        Options.MQTT.Port = credential.protocols.mqtt.port;
                        Options.MQTT.Username = credential.protocols.mqtt.username;
                        Options.MQTT.Password = credential.protocols.mqtt.password;
                    }
                }
                return true;
            }
            catch ( Exception ex )
            {
                return false;
            }
        }

        private void _connect()
        {
            try
            {
                if ( _mqttClient != null && _mqttClient.IsConnected )
                    return;
                /*{
                    Task t = _mqttClient.StopAsync();
                    t.Wait();
                }*/

                if ( Options == null )
                    return;

                if ( Options.ConnectType == ConnectType.DCCS )
                {
                    _getCredentialFromDCCS();
                }

                LastWillMessage lastWillMsg = new LastWillMessage();
                string payload = JsonConvert.SerializeObject( lastWillMsg );
                MqttApplicationMessage msg = new MqttApplicationMessage()
                {
                    Payload = Encoding.UTF8.GetBytes( payload ),
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                    Retain = true,
                    Topic = string.Format( MQTTTopic.ScadaConnTopic, Options.ScadaId )
                };

                string clientId = "EdgeAgent_" + Guid.NewGuid().ToString( "N" );
                var ob = new MqttClientOptionsBuilder();
                ob.WithClientId( clientId )
                .WithCredentials( Options.MQTT.Username, Options.MQTT.Password )
                .WithCleanSession()
                .WithWillMessage( msg );

                switch ( Options.MQTT.ProtocolType )
                {
                    case Protocol.TCP:
                        ob.WithTcpServer( Options.MQTT.HostName, Options.MQTT.Port );
                        break;
                    case Protocol.WebSocket:
                        ob.WithWebSocketServer( Options.MQTT.HostName );
                        break;
                    default:
                        ob.WithTcpServer( Options.MQTT.HostName, Options.MQTT.Port );
                        break;
                }

                if ( Options.UseSecure )
                    ob.WithTls();

                var mob = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay( TimeSpan.FromMilliseconds( Options.ReconnectInterval ) )
                .WithClientOptions( ob.Build() )
                .Build();

                _mqttClient.StartAsync( mob );
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
            }
        }

        private void _disconnect()
        {
            try
            {
                if ( _mqttClient != null && _mqttClient.IsConnected == false )
                    return;

                DisconnectMessage disconnectMsg = new DisconnectMessage();
                string payload = JsonConvert.SerializeObject( disconnectMsg );

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic( ( _options.Type == EdgeType.Gatway ) ? _scadaConnTopic : _deviceConnTopic )
                    .WithPayload( payload )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( true )
                    .Build();
                
                _mqttClient.PublishAsync( message ).ContinueWith( t => _mqttClient.StopAsync() );
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
            }
        }

        private bool _uploadConfig( ActionType action, EdgeConfig edgeConfig )
        {
            try
            {
                if ( _mqttClient.IsConnected == false )
                    return false;

                if ( edgeConfig == null )
                    return false;

                string payload = string.Empty;
                bool result = false;
                switch ( action )
                {
                    case ActionType.Create:
                        result = Converter.ConvertCreateOrUpdateConfig( edgeConfig, ref payload, _options.Heartbeat );
                        break;
                    case ActionType.Update:
                        result = Converter.ConvertCreateOrUpdateConfig( edgeConfig, ref payload, _options.Heartbeat );
                        break;
                    case ActionType.Delete:
                        result = Converter.ConvertDeleteConfig( edgeConfig, ref payload );
                        break;
                }

                if ( result )
                {
                    var message = new MqttApplicationMessageBuilder()
                    .WithTopic( _configTopic )
                    .WithPayload( payload )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( false )
                    .Build();

                    _mqttClient.PublishAsync( message );
                }
                return result;
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
                return false;
            }
        }

        private bool _sendData( EdgeData data )
        {
            try
            {
                if ( data == null )
                    return false;

                string payload = string.Empty;
                bool result = Converter.ConvertData( data, ref payload );
                if ( result )
                {
                    if ( _mqttClient.IsConnected == false && _recoverHelper != null )
                    {
                        // keep data for MQTT connected
                        _recoverHelper.Write( payload );
                        return false;
                    }

                    var message = new MqttApplicationMessageBuilder()
                    .WithTopic( _dataTopic )
                    .WithPayload( payload )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( false )
                    .Build();

                    _mqttClient.PublishAsync( message );
                }

                //_logger.Info( "Send Data: {0}", payload );

                return result;
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
                return false;
            }
        }

        private bool _sendDeviceStatus( EdgeDeviceStatus deviceStatus )
        {
            try
            {
                if ( _mqttClient.IsConnected == false )
                    return false;

                if ( deviceStatus == null )
                    return false;

                string payload = string.Empty;
                bool result = Converter.ConvertDeviceStatus( deviceStatus, ref payload );
                if ( result )
                {
                    var message = new MqttApplicationMessageBuilder()
                    .WithTopic( _scadaConnTopic )
                    .WithPayload( payload )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( true )
                    .Build();

                    _mqttClient.PublishAsync( message );
                }
                return result;
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
                return false;
            }
        }

        private void mqttClient_MessageReceived( object sender, MqttApplicationMessageReceivedEventArgs e )
        {
            try
            {
                if ( MessageReceived == null )
                    return;

                string payload = Encoding.UTF8.GetString( e.ApplicationMessage.Payload );
                //_logger.Info( "Recieved Message: {0}", payload );

                JObject jObj = JObject.Parse( payload );
                if ( jObj == null || jObj["d"] == null )
                    return;

                if ( jObj["d"]["Cmd"] != null )
                {
                    string cmd = ( string ) jObj["d"]["Cmd"];

                    var message = new BaseMessage();
                    switch ( cmd )
                    {
                        case "WV":
                            message = JsonConvert.DeserializeObject<WriteValueCommandMessage>( payload );
                            MessageReceived( sender, new MessageReceivedEventArgs( MessageType.WriteValue, message ) );
                            break;
                        case "WC":
                            MessageReceived( sender, new MessageReceivedEventArgs( MessageType.WriteConfig, message ) );
                            break;
                        case "DOn":
                            message = JsonConvert.DeserializeObject<DataOnCommandMessage>( payload );
                            MessageReceived( sender, new MessageReceivedEventArgs( MessageType.DataOn, message ) );
                            break;
                        case "DOf":
                            message = JsonConvert.DeserializeObject<DataOffCommandMessage>( payload );
                            MessageReceived( sender, new MessageReceivedEventArgs( MessageType.DataOff, message ) );
                            break;
                    }
                }
                else if ( jObj["d"]["Cfg"] != null )
                {
                    string cmd = ( string ) jObj["d"]["Cfg"];

                    var message = JsonConvert.DeserializeObject<ConfigAckMessage>( payload );
                    MessageReceived( this, new MessageReceivedEventArgs( MessageType.ConfigAck, message ) );
                }
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
            }
        }

        private void mqttClient_Connected( object sender, MqttClientConnectedEventArgs e )
        {
            try
            {
                //_logger.Info( "MQTT Connect Success !" );

                if ( string.IsNullOrEmpty( _options.ScadaId ) == false )
                {
                    string scadaCmdTopic = string.Format( "/wisepaas/scada/{0}/cmd", _options.ScadaId );
                    string deviceCmdTopic = string.Format( "/wisepaas/scada/{0}/{1}/cmd", _options.ScadaId, _options.DeviceId );

                    _configTopic = string.Format( "/wisepaas/scada/{0}/cfg", _options.ScadaId );
                    _dataTopic = string.Format( "/wisepaas/scada/{0}/data", _options.ScadaId );
                    _scadaConnTopic = string.Format( "/wisepaas/scada/{0}/conn", _options.ScadaId );
                    _deviceConnTopic = string.Format( "/wisepaas/scada/{0}/{1}/conn", _options.ScadaId, _options.DeviceId );
                    _ackTopic = string.Format( "/wisepaas/scada/{0}/ack", _options.ScadaId );
                    _cfgAckTopic = string.Format( "/wisepaas/scada/{0}/cfgack", _options.ScadaId );
                    if ( _options.Type == EdgeType.Gatway )
                        _cmdTopic = scadaCmdTopic;
                    else
                        _cmdTopic = deviceCmdTopic;
                }

                if ( _options.Heartbeat > 0 )
                {
                    if ( _heartbeatTimer == null )
                        _heartbeatTimer = new Timer();

                    _heartbeatTimer.Enabled = false;
                    _heartbeatTimer.Interval = _options.Heartbeat;
                }

                if ( Connected != null )
                    Connected( this, new EdgeAgentConnectedEventArgs( e.IsSessionPresent ) );

                // subscribe
                _mqttClient.SubscribeAsync( new TopicFilterBuilder().WithTopic( _cmdTopic ).WithAtLeastOnceQoS().Build() );
                _mqttClient.SubscribeAsync( new TopicFilterBuilder().WithTopic( _ackTopic ).WithAtLeastOnceQoS().Build() );

                // publish
                ConnectMessage connectMsg = new ConnectMessage();
                string payload = JsonConvert.SerializeObject( connectMsg );
                var message = new MqttApplicationMessageBuilder()
                .WithTopic( ( _options.Type == EdgeType.Gatway ) ? _scadaConnTopic : _deviceConnTopic )
                .WithPayload( payload )
                .WithAtLeastOnceQoS()
                .WithRetainFlag( true )
                .Build();

                _mqttClient.PublishAsync( message );

                // start heartbeat timer
                _heartbeatTimer.Enabled = true;
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
            }
        }

        private void mqttClient_Disconnected( object sender, MqttClientDisconnectedEventArgs e )
        {
            try
            {
                //_logger.Info( "MQTT Disonnected !" );
                Console.WriteLine( "Disonnected" );
                if ( Disconnected != null )
                    Disconnected( this, new DisconnectedEventArgs( e.ClientWasConnected, e.Exception ) );

                // refetch MQTT credential from DCCS
                if ( _options.ConnectType == ConnectType.DCCS )
                    _getCredentialFromDCCS();

                // stop heartbeat timer
                _heartbeatTimer.Enabled = false;
            }
            catch ( Exception ex )
            {
                //_logger.Error( ex.ToString() );
            }
        }

        #endregion

        #region >>> Public Method <<<

        public Task Connect()
        {
            return Task.Run( () => _connect() );
        }

        public Task Disconnect()
        {
            return Task.Run( () => _disconnect() );
        }

        public Task<bool> UploadConfig( ActionType action, EdgeConfig edgeConfig )
        {
            return Task.Run( () => _uploadConfig( action, edgeConfig ) );
        }

        public Task<bool> SendData( EdgeData data )
        {
            return Task.Run( () => _sendData( data ) );
        }

        public Task<bool> SendDeviceStatus( EdgeDeviceStatus deviceStatus )
        {
            return Task.Run( () => _sendDeviceStatus( deviceStatus ) );
        }

        #endregion
    }
}
