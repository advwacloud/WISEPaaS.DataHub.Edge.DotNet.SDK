using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK.Model
{
    public class ConfigMessage : BaseMessage
    {
        [JsonProperty( PropertyName = "d" )]
        public DObject D { get; set; }

        public ConfigMessage()
        {
            D = new DObject();
        }

        public class DObject
        {
            [JsonProperty( PropertyName = "Action" )]
            public ActionType Action { get; set; }
            [JsonProperty( PropertyName = "Scada" )]
            public Dictionary<string, ScadaObject> ScadaList { get; set; }

            public DObject()
            {
                Action = ActionType.Create;
            }
        }

        public class ScadaObject
        {
            [JsonProperty( PropertyName = "ID" )]
            public string Id { get; set; }
            [JsonProperty( PropertyName = "Name" )]
            public string Name { get; set; }
            [JsonProperty( PropertyName = "Desc" )]
            public string Description { get; set; }
            [JsonProperty( PropertyName = "PIP" )]
            public string PrimaryIP { get; set; }
            [JsonProperty( PropertyName = "BIP" )]
            public string BackupIP { get; set; }
            [JsonProperty( PropertyName = "PPort" )]
            public int? PrimaryPort { get; set; }
            [JsonProperty( PropertyName = "BPort" )]
            public int? BackupPort { get; set; }
            [JsonProperty( PropertyName = "Hbt" )]
            public int? Heartbeat { get; set; }
            [JsonProperty( PropertyName = "Type" )]
            public SCADAConfigType? Type { get; set; }

            [JsonProperty( PropertyName = "Device" )]
            public Dictionary<string, DeviceObject> DeviceList { get; set; }

            public ScadaObject()
            {
            }
        }

        public class DeviceObject
        {
            [JsonProperty( PropertyName = "Name" )]
            public string Name { get; set; }
            [JsonProperty( PropertyName = "Type" )]
            public string Type { get; set; }
            [JsonProperty( PropertyName = "Desc" )]
            public string Description { get; set; }
            [JsonProperty( PropertyName = "IP" )]
            public string IP { get; set; }
            [JsonProperty( PropertyName = "Port" )]
            public int? Port { get; set; }
            [JsonProperty( PropertyName = "PNbr" )]
            public int? ComPortNumber { get; set; }

            [JsonProperty( PropertyName = "Tag" )]
            public Dictionary<string, TagObject> TagList { get; set; }

            public DeviceObject()
            {
            }
        }

        public class TagObject
        {
            [JsonProperty( PropertyName = "Type" )]
            public TagType? Type { get; set; }
            [JsonProperty( PropertyName = "Desc" )]
            public string Description { get; set; }
            [JsonProperty( PropertyName = "RO" )]
            public int? ReadOnly { get; set; }
            [JsonProperty( PropertyName = "Ary" )]
            public int? ArraySize { get; set; }
            [JsonProperty( PropertyName = "Alm" )]
            public int? AlarmStatus { get; set; }
            [JsonProperty( PropertyName = "Log" )]
            public int? NeedLog { get; set; }
        }

        public class AnalogTagObject : TagObject
        {
            [JsonProperty( PropertyName = "SH" )]
            public double? SpanHigh { get; set; }
            [JsonProperty( PropertyName = "SL" )]
            public double? SpanLow { get; set; }
            [JsonProperty( PropertyName = "EU" )]
            public string EngineerUnit { get; set; }
            [JsonProperty( PropertyName = "IDF" )]
            public int? IntegerDisplayFormat { get; set; }
            [JsonProperty( PropertyName = "FDF" )]
            public int? FractionDisplayFormat { get; set; }

            [JsonProperty( PropertyName = "SCALE" )]
            public int? ScalingType { get; set; }
            [JsonProperty( PropertyName = "SF1" )]
            public double? ScalingFactor1 { get; set; }
            [JsonProperty( PropertyName = "SF2" )]
            public double? ScalingFactor2 { get; set; }

            public AnalogTagObject()
            {
            }
        }

        public class DiscreteTagObject : TagObject
        {
            [JsonProperty( PropertyName = "S0" )]
            public string State0 { get; set; }
            [JsonProperty( PropertyName = "S1" )]
            public string State1 { get; set; }
            [JsonProperty( PropertyName = "S2" )]
            public string State2 { get; set; }
            [JsonProperty( PropertyName = "S3" )]
            public string State3 { get; set; }
            [JsonProperty( PropertyName = "S4" )]
            public string State4 { get; set; }
            [JsonProperty( PropertyName = "S5" )]
            public string State5 { get; set; }
            [JsonProperty( PropertyName = "S6" )]
            public string State6 { get; set; }
            [JsonProperty( PropertyName = "S7" )]
            public string State7 { get; set; }
            
            public DiscreteTagObject()
            {
            }
        }

        public class TextTagObject : TagObject
        {
            public TextTagObject()
            {
            }
        }
    }
}
