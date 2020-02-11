﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class TimeSyncCommand
    {
        public DateTime UTCTime { get; set; }

        public TimeSyncCommand()
        {
            UTCTime = DateTime.UtcNow;
        }
    }
}
