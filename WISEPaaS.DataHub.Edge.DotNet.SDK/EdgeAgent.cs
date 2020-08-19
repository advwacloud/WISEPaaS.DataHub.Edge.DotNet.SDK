using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Implementations;
using MQTTnet.ManagedClient;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WISEPaaS.DataHub.Edge.DotNet.SDK.Model;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK
{
    public class EdgeAgent
    {
        public const int DEAFAULT_HEARTBEAT_INTERVAL = 60000;
        public const int DEAFAULT_DATARECOVER_INTERVAL = 3000;
        public const int DEAFAULT_DATARECOVER_COUNT = 10;

        private ManagedMqttClient _mqttClient;
        private DataRecoverHelper _recoverHelper;

        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private string _configTopic;
        private string _dataTopic;
        private string _nodeConnTopic;
        private string _deviceConnTopic;
        private string _cmdTopic;
        private string _ackTopic;
        private string _cfgAckTopic;
        private string _dataAdustTopic;

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

            if( _options.Heartbeat > 0 )
            {
                _heartbeatTimer = new Timer();
                _heartbeatTimer.Interval = _options.Heartbeat;
                _heartbeatTimer.Elapsed += _heartbeatTimer_Elapsed;
            }

            if ( options.DataRecover )
            {
                _recoverHelper = new DataRecoverHelper();
                _dataRecoverTimer = new Timer();
                _dataRecoverTimer.Interval = DEAFAULT_DATARECOVER_INTERVAL;
                _dataRecoverTimer.Elapsed += _dataRecoverTimer_Elapsed;
            }

            MqttTcpChannel.CustomCertificateValidationCallback = ( x509Certificate, x509Chain, sslPolicyErrors, mqttClientTcpOptions ) => { return true; };
        }

        #region >>> Private Method <<<

        // for self-signed cert
        private bool checkValidationResult( object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors )
        {
            return true;
        }

        private async void _dataRecoverTimer_Elapsed( object sender, ElapsedEventArgs e )
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

                    await _mqttClient.PublishAsync( message );
                }
            }
        }

        private async void _heartbeatTimer_Elapsed( object sender, ElapsedEventArgs e )
        {
            HeartbeatMessage heartbeatMsg = new HeartbeatMessage();
            string payload = JsonConverter.SerializeObject( heartbeatMsg );

            var message = new MqttApplicationMessageBuilder()
            .WithTopic( ( _options.Type == EdgeType.Gateway ) ? _nodeConnTopic : _deviceConnTopic )
            .WithPayload( payload )
            .WithAtLeastOnceQoS()
            .WithRetainFlag( true )
            .Build();

            await _mqttClient.PublishAsync( message );
        }

        private bool _getCredentialFromDCCS()
        {
            try
            {
                _logger.Info( "Fetch MQTT Credentials form DCCS API !" );
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
                _logger.Error( "Fetch MQTT Credentials Error ! " + ex.ToString() );
                return false;
            }
        }

        private async Task _connect()
        {
            try
            {
                if ( _mqttClient != null && _mqttClient.IsConnected )
                    return;

                if ( Options == null )
                    return;

                if ( Options.ConnectType == ConnectType.DCCS )
                {
                    _getCredentialFromDCCS();
                }

                LastWillMessage lastWillMsg = new LastWillMessage();
                string payload = JsonConverter.SerializeObject( lastWillMsg );
                MqttApplicationMessage msg = new MqttApplicationMessage()
                {
                    Payload = Encoding.UTF8.GetBytes( payload ),
                    QualityOfServiceLevel = MqttQualityOfServiceLevel.AtLeastOnce,
                    Retain = true,
                    Topic = string.Format( MQTTTopic.NodeConnTopic, Options.NodeId )
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

                await _mqttClient.StartAsync( mob );
            }
            catch ( Exception ex )
            {
                _logger.Error( "Connect Error ! " + ex.ToString() );
            }
        }

        private async Task _disconnect()
        {
            try
            {
                if ( _mqttClient != null && _mqttClient.IsConnected == false )
                    return;

                DisconnectMessage disconnectMsg = new DisconnectMessage();
                string payload = JsonConverter.SerializeObject( disconnectMsg );

                var message = new MqttApplicationMessageBuilder()
                    .WithTopic( ( _options.Type == EdgeType.Gateway ) ? _nodeConnTopic : _deviceConnTopic )
                    .WithPayload( payload )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( true )
                    .Build();

                await _mqttClient.PublishAsync( message ).ContinueWith( t => _mqttClient.StopAsync() );
            }
            catch ( Exception ex )
            {
                _logger.Error( "Disconnect Error ! " + ex.ToString() );
            }
        }

        private async Task<bool> _uploadConfig( ActionType action, EdgeConfig edgeConfig )
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
                    case ActionType.Update:
                    case ActionType.Delsert:
                        result = Converter.ConvertWholeConfig( action, Options.NodeId, edgeConfig, ref payload, _options.Heartbeat );
                        break;
                    case ActionType.Delete:
                        result = Converter.ConvertDeleteConfig( Options.NodeId, edgeConfig, ref payload );
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

                    await _mqttClient.PublishAsync( message );
                }
                return result;
            }
            catch ( Exception ex )
            {
                _logger.Error( "Upload Config Error ! " + ex.ToString() );
                return false;
            }
        }

        private async Task<bool> _sendData( EdgeData data )
        {
            try
            {
                if ( data == null )
                    return false;

                List<string> payloads = new List<string>();
                bool result = Converter.ConvertData( data, ref payloads );
                if ( result )
                {
                    if ( _mqttClient.IsConnected == false )
                    {
                        // keep data for MQTT connected
                        _recoverHelper.Write( payloads );
                        return false;
                    }

                    foreach ( var payload in payloads )
                    {
                        var message = new MqttApplicationMessageBuilder()
                            .WithTopic( _dataTopic )
                            .WithPayload( payload )
                            .WithAtLeastOnceQoS()
                            .WithRetainFlag( false )
                            .Build();

                        await _mqttClient.PublishAsync( message );
                    }

                    //_logger.Info( "Send Data: {0}", payload );
                    return true;
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "Send Data Error ! " + ex.ToString() );
            }
            return false;
        }

        private async Task<bool> _sendDeviceStatus( EdgeDeviceStatus deviceStatus )
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
                    .WithTopic( _nodeConnTopic )
                    .WithPayload( payload )
                    .WithAtLeastOnceQoS()
                    .WithRetainFlag( true )
                    .Build();

                    await _mqttClient.PublishAsync( message );
                    return true;
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "Send Device Status Error ! " + ex.ToString() );
            }
            return false;
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

                dynamic obj = jObj as dynamic;
                if ( jObj["d"]["Cmd"] != null )
                {
                    switch ( ( string ) obj.d.Cmd )
                    {
                        case "WV":
                            WriteValueCommand wvcMsg = new WriteValueCommand();
                            foreach ( JProperty devObj in obj.d.Val )
                            {
                                WriteValueCommand.Device device = new WriteValueCommand.Device();
                                device.Id = devObj.Name;
                                foreach ( JProperty tagObj in devObj.Value )
                                {
                                    WriteValueCommand.Tag tag = new WriteValueCommand.Tag();
                                    tag.Name = tagObj.Name;
                                    tag.Value = tagObj.Value;
                                    device.TagList.Add( tag );
                                }
                                wvcMsg.DeviceList.Add( device );
                            }
                            //message = JsonConvert.DeserializeObject<WriteValueCommandMessage>( payload );
                            MessageReceived( sender, new MessageReceivedEventArgs( MessageType.WriteValue, wvcMsg ) );
                            break;
                        case "WC":
                            //MessageReceived( sender, new MessageReceivedEventArgs( MessageType.WriteConfig, message ) );
                            break;
                        case "TSyn":
                            TimeSyncCommand tscMsg = new TimeSyncCommand();
                            DateTime miniDateTime = new DateTime( 1970, 1, 1, 0, 0, 0, 0 );
                            tscMsg.UTCTime = miniDateTime.AddSeconds( obj.d.UTC.Value );
                            MessageReceived( sender, new MessageReceivedEventArgs( MessageType.TimeSync, tscMsg ) );
                            break;
                    }
                }
                else if ( jObj["d"]["Cfg"] != null )
                {
                    ConfigAck ackMsg = new ConfigAck();
                    ackMsg.Result = Convert.ToBoolean( obj.d.Cfg.Value );
                    MessageReceived( this, new MessageReceivedEventArgs( MessageType.ConfigAck, ackMsg ) );
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "Message Received Error ! " + ex.ToString() );
            }
        }

        private async void mqttClient_Connected( object sender, MqttClientConnectedEventArgs e )
        {
            try
            {
                _logger.Info( "MQTT Connect Success !" );

                if ( string.IsNullOrEmpty( _options.NodeId ) == false )
                {
                    string nodeCmdTopic = string.Format( MQTTTopic.NodeCmdTopic, _options.NodeId );
                    string deviceCmdTopic = string.Format( MQTTTopic.DeviceCmdTopic, _options.NodeId, _options.DeviceId );

                    _configTopic = string.Format( MQTTTopic.ConfigTopic, _options.NodeId );
                    _dataTopic = string.Format( MQTTTopic.DataTopic, _options.NodeId );
                    _nodeConnTopic = string.Format( MQTTTopic.NodeConnTopic, _options.NodeId );
                    _deviceConnTopic = string.Format( MQTTTopic.DeviceConnTopic, _options.NodeId, _options.DeviceId );
                    _ackTopic = string.Format( MQTTTopic.AckTopic, _options.NodeId );
                    _cfgAckTopic = string.Format( MQTTTopic.CfgAckTopic, _options.NodeId );
                    _dataAdustTopic = string.Format( MQTTTopic.DataAdjustTopic, _options.NodeId );

                    if ( _options.Type == EdgeType.Gateway )
                        _cmdTopic = nodeCmdTopic;
                    else
                        _cmdTopic = deviceCmdTopic;
                }

                // subscribe
                await _mqttClient.SubscribeAsync( new TopicFilterBuilder().WithTopic( _cmdTopic ).WithAtLeastOnceQoS().Build() );
                await _mqttClient.SubscribeAsync( new TopicFilterBuilder().WithTopic( _ackTopic ).WithAtLeastOnceQoS().Build() );

                if ( Connected != null )
                    Connected( this, new EdgeAgentConnectedEventArgs( e.IsSessionPresent ) );

                // publish
                ConnectMessage connectMsg = new ConnectMessage();
                string payload = JsonConverter.SerializeObject( connectMsg );
                var message = new MqttApplicationMessageBuilder()
                .WithTopic( ( _options.Type == EdgeType.Gateway ) ? _nodeConnTopic : _deviceConnTopic )
                .WithPayload( payload )
                .WithAtLeastOnceQoS()
                .WithRetainFlag( true )
                .Build();

                await _mqttClient.PublishAsync( message );

                // start heartbeat and recover timer
                if ( _heartbeatTimer != null )
                    _heartbeatTimer.Enabled = true;

                if ( _dataRecoverTimer != null )
                    _dataRecoverTimer.Enabled = true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "MQTT Connected Error ! " + ex.ToString() );
            }
        }

        private void mqttClient_Disconnected( object sender, MqttClientDisconnectedEventArgs e )
        {
            try
            {
                _logger.Info( "MQTT Connection Disonnected !" );

                if ( Disconnected != null )
                    Disconnected( this, new DisconnectedEventArgs( e.ClientWasConnected, e.Exception ) );

                // refetch MQTT credential from DCCS
                if ( _options.ConnectType == ConnectType.DCCS )
                    _getCredentialFromDCCS();

                // stop heartbeat and recover timer
                if ( _heartbeatTimer != null )
                    _heartbeatTimer.Enabled = false;

                if ( _dataRecoverTimer != null )
                    _dataRecoverTimer.Enabled = false;
            }
            catch ( Exception ex )
            {
                _logger.Error( "MQTT Disconnected Error ! " + ex.ToString() );
            }
        }

        private async Task<bool> _updateData( EdgeUpdateData data, bool upsert = false )
        {
            try
            {
                if ( _mqttClient.IsConnected == false )
                    return false;

                if ( data == null )
                    return false;

                string payload = string.Empty;
                bool result = Converter.ConvertUpdateData( data, upsert, ref payload );
                if ( result )
                {
                    var message = new MqttApplicationMessageBuilder()
                        .WithTopic( _dataAdustTopic )
                        .WithPayload( payload )
                        .WithAtLeastOnceQoS()
                        .WithRetainFlag( false )
                        .Build();

                    await _mqttClient.PublishAsync( message );
                    return true;
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "Update Data Error ! " + ex.ToString() );
            }
            return false;
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

        public Task<bool> UpdateData( EdgeUpdateData data, bool upsert = false )
        {
            return Task.Run( () => _updateData( data, upsert ) );
        }

        #endregion
    }
}
