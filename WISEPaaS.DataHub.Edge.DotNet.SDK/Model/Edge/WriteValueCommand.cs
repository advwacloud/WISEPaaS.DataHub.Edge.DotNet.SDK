using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class WriteValueCommand
    {
        public HashSet<Device> DeviceList { get; set; }
        public DateTime Timestamp { get; set; }

        public WriteValueCommand()
        {
            DeviceList = new HashSet<Device>();
            Timestamp = DateTime.UtcNow;
        }

        public class Device
        {
            public string Id { get; set; }
            public HashSet<Tag> TagList { get; set; }

            public Device()
            {
                Id = string.Empty;
                TagList = new HashSet<Tag>();
            }
        }

        public class Tag
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public Tag()
            {
                Name = string.Empty;
                Value = new object();
            }
        }
    }
}
