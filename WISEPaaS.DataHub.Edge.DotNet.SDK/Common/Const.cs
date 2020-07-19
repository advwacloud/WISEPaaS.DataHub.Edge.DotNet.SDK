using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK
{
    public class MQTTTopic
    {
        public const string ConfigTopic = "/wisepaas/scada/{0}/cfg";
        public const string DataTopic = "/wisepaas/scada/{0}/data";
        public const string NodeConnTopic = "/wisepaas/scada/{0}/conn";
        public const string DeviceConnTopic = "/wisepaas/scada/{0}/{1}/conn";
        public const string NodeCmdTopic = "/wisepaas/scada/{0}/cmd";
        public const string DeviceCmdTopic = "/wisepaas/scada/{0}/{1}/cmd";
        public const string AckTopic = "/wisepaas/scada/{0}/ack";
        public const string CfgAckTopic = "/wisepaas/scada/{0}/cfgack";
        public const string DataAdjustTopic = "/wisepaas/scada/{0}/data/adjust";
    }

    public class Limit
    {
        public const int DataMaxTagCount = 100;
    }
}
