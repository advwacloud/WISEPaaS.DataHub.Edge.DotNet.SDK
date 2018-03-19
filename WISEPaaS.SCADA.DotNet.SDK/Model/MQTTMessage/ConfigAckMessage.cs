using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK.Model
{
    public class ConfigAckMessage : BaseMessage
    {
        [JsonProperty( PropertyName = "d" )]
        public DObject D { get; set; }

        public ConfigAckMessage()
        {
            D = new DObject();
        }

        public class DObject
        {
            [JsonProperty( PropertyName = "Cfg" )]
            public ResultType Cfg { get; set; }

            public DObject()
            {
                Cfg = ResultType.Fail;
            }
        }
    }
}
