using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK.Model
{
    public class WriteValueCommandMessage : BaseMessage
    {
        [JsonProperty( PropertyName = "d" )]
        public DObject D { get; set; }

        public WriteValueCommandMessage()
        {
            D = new DObject();
        }

        public class DObject
        {
            [JsonProperty( PropertyName = "Cmd" )]
            public string Cmd { get; set; }
            [JsonProperty( PropertyName = "Val" )]
            public Dictionary<string, object> Val { get; set; }

            public DObject()
            {
                Cmd = string.Empty;
                Val = new Dictionary<string, object>();
            }
        }
    }
}
