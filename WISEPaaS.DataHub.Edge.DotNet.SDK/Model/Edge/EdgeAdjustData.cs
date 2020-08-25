using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class EdgeUpdateData
    {
        public HashSet<Tag> TagList { get; set; }

        public EdgeUpdateData()
        {
            TagList = new HashSet<Tag>();
        }

        public class Tag
        {
            public string DeviceId { get; set; }
            public string TagName { get; set; }
            public object Value { get; set; }
            public int? Index { get; set; }
            public DateTime Timestamp { get; set; }

            public Tag()
            {
                DeviceId = string.Empty;
                TagName = string.Empty;
                Value = new object();
                Timestamp = DateTime.UtcNow;
            }
        }
    }
}
