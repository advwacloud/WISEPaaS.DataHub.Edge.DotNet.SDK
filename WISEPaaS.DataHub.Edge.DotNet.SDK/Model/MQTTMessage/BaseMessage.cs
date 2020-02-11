using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK.Model
{
    public class BaseMessage
    {
        [JsonProperty( PropertyName = "ts" )]
        public DateTime Timestamp { get; set; }

        public BaseMessage()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}
