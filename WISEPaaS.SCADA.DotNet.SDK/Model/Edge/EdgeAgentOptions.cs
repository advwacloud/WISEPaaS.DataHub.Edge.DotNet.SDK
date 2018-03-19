using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK.Model
{
    public class EdgeAgentOptions
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Protocol ProtocolType { get; set; }
        public bool UseSecure { get; set; }
        public bool AutoReconnect { get; set; }
        public int ReconnectInterval { get; set; }
        public string ScadaId { get; set; }
        public string DeviceId { get; set; }
        public EdgeType Type { get; set; }
        public int Heartbeat { get; set; }
        public bool DataRecover { get; set; }

        public EdgeAgentOptions()
        {
            HostName = string.Empty;
            Port = 1883;
            Username = string.Empty;
            Password = string.Empty;
            ProtocolType = Protocol.TCP;
            UseSecure = false;
            AutoReconnect = false;
            ReconnectInterval = 1000;
            ScadaId = string.Empty;
            DeviceId = string.Empty;
            Type = EdgeType.Gatway;
            Heartbeat = EdgeAgent.DEAFAULT_HEARTBEAT_INTERVAL;
            DataRecover = true;
        }

    }

    public enum Protocol
    {
        TCP,
        WebSocket,
    }
}
