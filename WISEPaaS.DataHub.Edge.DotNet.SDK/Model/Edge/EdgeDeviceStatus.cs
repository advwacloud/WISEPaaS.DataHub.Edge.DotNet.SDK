using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class EdgeDeviceStatus
    {
        public HashSet<Device> DeviceList { get; set; }
        public DateTime Timestamp { get; set; }

        public EdgeDeviceStatus()
        {
            DeviceList = new HashSet<Device>();
            Timestamp = DateTime.UtcNow;
        }

        public class Device
        {
            public string Id { get; set; }
            public Status Status { get; set; }

            public Device()
            {
                Id = string.Empty;
                Status = Status.Offline;
            }
        }
    }
}
