﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK
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
        Gatway = 0,
        Device = 1
    }

    public enum MessageType
    {
        DataOn = 0,
        DataOff,
        WriteValue,
        WriteConfig,
        //TimeSync,
        ConfigAck
    }

    public enum SCADAConfigType
    {
        SCADA = 1,
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
        Delete = 3
    }
}
