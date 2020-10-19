using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class DataCache
    {
        public object Value;
        public DateTime Timestamp;

        public DataCache()
        {
            Value = new object();
            Timestamp = DateTime.Now;
        }
    }
}
