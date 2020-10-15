using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class ConfigCache
    {
        public Dictionary<string, Dictionary<string, TagObject>> DeviceList;

        public ConfigCache()
        {
            DeviceList = new Dictionary<string, Dictionary<string, TagObject>>();
        }

        public class TagObject
        {
            [JsonProperty( PropertyName = "SWVC" )]
            public bool? SendWhenValueChanged { get; set; }

            public TagObject()
            {
                SendWhenValueChanged = false;
            }
        }

        public class AnalogTagObject : TagObject
        {
            [JsonProperty( PropertyName = "SH" )]
            public double? SpanHigh { get; set; }

            [JsonProperty( PropertyName = "SL" )]
            public double? SpanLow { get; set; }
            
            [JsonProperty( PropertyName = "FDF" )]
            public int? FractionDisplayFormat { get; set; }

            [JsonProperty( PropertyName = "Deadband" )]
            public int? Deadband { get; set; }

            public AnalogTagObject()
            {
            }
        }
    }
}
