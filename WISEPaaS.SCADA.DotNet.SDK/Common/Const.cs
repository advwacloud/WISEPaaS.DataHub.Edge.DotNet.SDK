using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK
{
    public class MQTTTopic
    {
        public const string ConfigTopic = "/wisepaas/scada/{0}/cfg";
        public const string DataTopic = "/wisepaas/scada/{0}/data";
        public const string ScadaConnTopic = "/wisepaas/scada/{0}/conn";
        public const string DeviceConnTopic = "/wisepaas/scada/{0}/{1}/conn";
        public const string ScadaCmdTopic = "/wisepaas/scada/{0}/cmd";
        public const string DeviceCmdTopic = "/wisepaas/scada/{0}/{1}/cmd";
        public const string AckTopic = "/wisepaas/scada/{0}/ack";
        public const string CfgAckTopic = "/wisepaas/scada/{0}/cfgack";
    }

    public class DataRecover
    {
        public const string DatabaseFileName = "recover.sqlite";
    }

    public class Limit
    {
        public const int DataMaxTagCount = 100;
    }
}
