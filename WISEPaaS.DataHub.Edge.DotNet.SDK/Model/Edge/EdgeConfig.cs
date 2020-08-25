using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class EdgeConfig
    {
        public NodeConfig Node { get; set; }

        public EdgeConfig()
        {
            Node = new NodeConfig();
        }

        public class NodeConfig
        {
            public NodeConfigType? Type { get; set; }
            public HashSet<DeviceConfig> DeviceList { get; set; }

            public NodeConfig()
            {
                DeviceList = new HashSet<DeviceConfig>();
            }
        }

        public class DeviceConfig
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public string RetentionPolicyName { get; set; }
            public HashSet<AnalogTagConfig> AnalogTagList { get; set; }
            public HashSet<DiscreteTagConfig> DiscreteTagList { get; set; }
            public HashSet<TextTagConfig> TextTagList { get; set; }

            public DeviceConfig()
            {
                AnalogTagList = new HashSet<AnalogTagConfig>();
                DiscreteTagList = new HashSet<DiscreteTagConfig>();
                TextTagList = new HashSet<TextTagConfig>();
            }
        }

        public class TagConfig
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool? ReadOnly { get; set; }
            public int? ArraySize { get; set; }

            public bool SendWhenValueChanged { get; set; }

            public TagConfig()
            {
                Name = string.Empty;
                SendWhenValueChanged = false;
            }
        }

        public class AnalogTagConfig : TagConfig
        {
            public double? SpanHigh { get; set; }
            public double? SpanLow { get; set; }
            public string EngineerUnit { get; set; }
            public int? IntegerDisplayFormat { get; set; }
            public int? FractionDisplayFormat { get; set; }

            public AnalogTagConfig()
            {
            }
        }

        public class DiscreteTagConfig : TagConfig
        {
            public string State0 { get; set; }
            public string State1 { get; set; }
            public string State2 { get; set; }
            public string State3 { get; set; }
            public string State4 { get; set; }
            public string State5 { get; set; }
            public string State6 { get; set; }
            public string State7 { get; set; }
 
            public DiscreteTagConfig()
            {
            }
        }

        public class TextTagConfig : TagConfig
        {
            public TextTagConfig()
            {
            }
        }
    }
}
