using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK
{
    public enum Protocol
    {
        TCP,
        WebSocket,
    }

    public enum ConnectType
    {
        MQTT,
        DCCS,
    }

    public enum EdgeType
    {
        Gateway = 0,
        Device = 1
    }

    public enum MessageType
    {
        WriteValue = 0,
        WriteConfig,
        TimeSync,
        ConfigAck
    }

    public enum NodeConfigType
    {
        Node = 1,
        Gateway = 2,
        VirtualGroup = 3
    }

    public enum TagType
    {
        Analog = 1,
        Discrete = 2,
        Text = 3
    }

    public enum Status
    {
        Offline = 0,
        Online = 1
    }

    public enum ResultType
    {
        Fail = 0,
        Success = 1
    }

    public enum ActionType
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Delsert = 4
    }

    public enum DataAdjustActionType
    {
        [EnumMember( Value = "update" )]
        Update,
        [EnumMember( Value = "upsert" )]
        Upsert,
        [EnumMember( Value = "delete" )]
        Delete
    };
}
