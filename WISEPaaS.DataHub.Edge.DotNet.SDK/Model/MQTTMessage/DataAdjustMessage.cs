using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class DataAdjustMessage : BaseMessage
    {
        [JsonProperty( PropertyName = "d" )]
        public DObject D { get; set; }

        public DataAdjustMessage()
        {
            D = new DObject();
        }

        public class DObject
        {
            [JsonProperty( PropertyName = "action" )]
            [JsonConverter( typeof( StringEnumConverter ) )]
            [DefaultValue( "" )]
            public DataAdjustActionType Action { get; set; }

            [JsonProperty( PropertyName = "tags" )]
            public HashSet<TagObject> TagList { get; set; }

            public DObject()
            {
                TagList = new HashSet<TagObject>();
            }
        }

        public class TagObject
        {
            [JsonProperty( PropertyName = "deviceId" )]
            public string DeviceId { get; set; }
            [JsonProperty( PropertyName = "tagName" )]
            public string TagName { get; set; }
            [JsonProperty( PropertyName = "value" )]
            public object Value { get; set; }
            [JsonProperty( PropertyName = "index" )]
            public int? Index { get; set; }
            [JsonProperty( PropertyName = "ts" )]
            public DateTime Timestamp { get; set; }

            public TagObject()
            {
                DeviceId = string.Empty;
                TagName = string.Empty;
                Timestamp = DateTime.UtcNow;
            }
        }
    }
}
