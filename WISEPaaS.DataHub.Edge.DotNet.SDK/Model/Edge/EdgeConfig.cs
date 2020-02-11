using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class EdgeConfig
    {
        public ScadaConfig Scada { get; set; }

        public EdgeConfig()
        {
            Scada = new ScadaConfig();
        }

        public class ScadaConfig
        {
            public string PrimaryIP { get; set; }
            public string BackupIP { get; set; }
            public int? PrimaryPort { get; set; }
            public int? BackupPort { get; set; }
            public SCADAConfigType? Type { get; set; }

            public List<DeviceConfig> DeviceList { get; set; }

            public ScadaConfig()
            {
            }
        }

        public class DeviceConfig
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int? ComPortNumber { get; set; }
            public string Type { get; set; }
            public string Description { get; set; }
            public string IP { get; set; }
            public int? Port { get; set; }
            public string RetentionPolicyName { get; set; }
            public List<AnalogTagConfig> AnalogTagList { get; set; }
            public List<DiscreteTagConfig> DiscreteTagList { get; set; }
            public List<TextTagConfig> TextTagList { get; set; }

            public DeviceConfig()
            {
            }
        }

        public class TagConfig
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool? ReadOnly { get; set; }
            public int? ArraySize { get; set; }

            public TagConfig()
            {
                Name = string.Empty;
            }
        }

        public class AnalogTagConfig : TagConfig
        {
            public double? SpanHigh { get; set; }
            public double? SpanLow { get; set; }
            public string EngineerUnit { get; set; }
            public int? IntegerDisplayFormat { get; set; }
            public int? FractionDisplayFormat { get; set; }

            public int? ScalingType { get; set; }
            public double? ScalingFactor1 { get; set; }
            public double? ScalingFactor2 { get; set; }

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
