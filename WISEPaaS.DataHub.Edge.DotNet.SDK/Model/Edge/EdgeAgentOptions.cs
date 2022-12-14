using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class EdgeAgentOptions
    {
        public bool AutoReconnect { get; set; }
        public int ReconnectInterval { get; set; }
        public string NodeId { get; set; }
        public string DeviceId { get; set; }
        public EdgeType Type { get; set; }
        private int _heartbeat;
        public int Heartbeat
        {
            get { return _heartbeat; }
            set { _heartbeat = ( value < EdgeAgent.DEAFAULT_HEARTBEAT_INTERVAL ) ? EdgeAgent.DEAFAULT_HEARTBEAT_INTERVAL : value; }
        }
        public bool DataRecover { get; set; }
        public ConnectType ConnectType { get; set; }
        public bool UseSecure { get; set; }

        public MQTTOptions MQTT;
        public DCCSOptions DCCS;

        public EdgeAgentOptions()
        {
            AutoReconnect = false;
            ReconnectInterval = 1000;
            NodeId = string.Empty;
            DeviceId = string.Empty;
            Type = EdgeType.Gateway;
            Heartbeat = EdgeAgent.DEAFAULT_HEARTBEAT_INTERVAL;
            DataRecover = true;
            ConnectType = ConnectType.DCCS;
            UseSecure = false;

            MQTT = new MQTTOptions();
            DCCS = new DCCSOptions();
        }
    }

    public class DCCSOptions
    {
        public string CredentialKey { get; set; }
        public string APIUrl { get; set; }

        public DCCSOptions()
        { }

        public DCCSOptions( string key, string url )
        {
            CredentialKey = key;
            APIUrl = url;
        }
    }

    public class MQTTOptions
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Protocol ProtocolType { get; set; }

        public MQTTOptions()
        {
            HostName = string.Empty;
            Port = 1883;
            Username = string.Empty;
            Password = string.Empty;
            ProtocolType = Protocol.TCP;
        }

        public MQTTOptions( string host, int port, string username, string password, Protocol protocol = Protocol.TCP )
        {
            HostName = host;
            Port = port;
            Username = username;
            Password = password;
            ProtocolType = protocol;
        }
    }

}
