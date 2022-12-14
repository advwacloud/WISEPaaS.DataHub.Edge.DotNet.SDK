using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
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
            public Dictionary<string, NodeObject> NodeList { get; set; }

            public DObject()
            {
                Action = ActionType.Create;
            }
        }

        public class NodeObject
        {
            [JsonProperty( PropertyName = "Hbt" )]
            public int? Heartbeat { get; set; }

            [JsonProperty( PropertyName = "Device" )]
            public Dictionary<string, DeviceObject> DeviceList { get; set; }

            public NodeObject()
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
            [DefaultValue( "" )]
            public string Description { get; set; }

            [JsonProperty( PropertyName = "RP" )]
            [DefaultValue( null )]
            public string RetentionPolicyName { get; set; }

            [JsonProperty( PropertyName = "Tag" )]
            public Dictionary<string, TagObject> TagList { get; set; }

            public DeviceObject()
            {
            }
        }

        public class TagObject
        {
            [JsonProperty( PropertyName = "Type" )]
            public TagType Type { get; set; }

            [JsonProperty( PropertyName = "Desc" )]
            [DefaultValue( "" )]
            public string Description { get; set; }

            [JsonProperty( PropertyName = "RO" )]
            [DefaultValue( 0 )]
            public int ReadOnly { get; set; }

            [JsonProperty( PropertyName = "Ary" )]
            [DefaultValue( 0 )]
            public int ArraySize { get; set; }
        }

        public class AnalogTagObject : TagObject
        {
            [JsonProperty( PropertyName = "SH" )]
            [DefaultValue( 1000.0 )]
            public double SpanHigh { get; set; }

            [JsonProperty( PropertyName = "SL" )]
            public double SpanLow { get; set; }

            [JsonProperty( PropertyName = "EU" )]
            [DefaultValue( "" )]
            public string EngineerUnit { get; set; }

            [JsonProperty( PropertyName = "IDF" )]
            [DefaultValue( 4 )]
            public int IntegerDisplayFormat { get; set; }

            [JsonProperty( PropertyName = "FDF" )]
            [DefaultValue( 2 )]
            public int FractionDisplayFormat { get; set; }

            public AnalogTagObject()
            {
            }
        }

        public class DiscreteTagObject : TagObject
        {
            [JsonProperty( PropertyName = "S0" )]
            [DefaultValue( "" )]
            public string State0 { get; set; }

            [JsonProperty( PropertyName = "S1" )]
            [DefaultValue( "" )]
            public string State1 { get; set; }

            [JsonProperty( PropertyName = "S2" )]
            [DefaultValue( "" )]
            public string State2 { get; set; }

            [JsonProperty( PropertyName = "S3" )]
            [DefaultValue( "" )]
            public string State3 { get; set; }

            [JsonProperty( PropertyName = "S4" )]
            [DefaultValue( "" )]
            public string State4 { get; set; }

            [JsonProperty( PropertyName = "S5" )]
            [DefaultValue( "" )]
            public string State5 { get; set; }

            [JsonProperty( PropertyName = "S6" )]
            [DefaultValue( "" )]
            public string State6 { get; set; }

            [JsonProperty( PropertyName = "S7" )]
            [DefaultValue( "" )]
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

        public class TagObjectConverter : CustomCreationConverter<TagObject>
        {
            public override TagObject Create( Type objectType )
            {
                throw new NotImplementedException();
            }

            public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
            {
                JObject jo = JObject.Load( reader );
                TagObject tagObj = new TagObject();
                switch ( jo["Type"].ToObject<TagType>() )
                {
                    case TagType.Analog:
                        tagObj = new AnalogTagObject();
                        break;
                    case TagType.Discrete:
                        tagObj = new DiscreteTagObject();
                        break;
                    case TagType.Text:
                        tagObj = new TextTagObject();
                        break;
                }
                serializer.Populate( jo.CreateReader(), tagObj );
                return tagObj;
            }
        }
    }


}
