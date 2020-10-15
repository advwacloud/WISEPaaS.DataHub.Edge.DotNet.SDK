using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WISEPaaS.DataHub.Edge.DotNet.SDK.Model;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK
{
    public class Converter
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Converter()
        { }

        public static string ConvertWholeConfig( ActionType action, string nodeId, int heartbeat, EdgeConfig config )
        {
            string payload = string.Empty;
            try
            {
                if ( config == null )
                    return payload;

                ConfigMessage msg = new ConfigMessage();
                msg.D.Action = action;
                msg.D.NodeList = new Dictionary<string, ConfigMessage.NodeObject>();

                ConfigMessage.NodeObject nodeObj = new ConfigMessage.NodeObject()
                {
                    Heartbeat = heartbeat / 1000,
                    Type = NodeConfigType.Node
                };

                if ( config.Node.DeviceList != null )
                {
                    nodeObj.DeviceList = new Dictionary<string, ConfigMessage.DeviceObject>();
                    foreach ( var device in config.Node.DeviceList )
                    {
                        ConfigMessage.DeviceObject deviceObj = new ConfigMessage.DeviceObject()
                        {
                            Name = device.Name,
                            Type = device.Type,
                            Description = device.Description,
                            RetentionPolicyName = device.RetentionPolicyName
                        };


                        if ( device.AnalogTagList != null )
                        {
                            foreach ( var analogTag in device.AnalogTagList )
                            {
                                ConfigMessage.AnalogTagObject analogTagObject = new ConfigMessage.AnalogTagObject()
                                {
                                    Type = TagType.Analog,
                                    Description = analogTag.Description,
                                    ReadOnly = ( analogTag.ReadOnly != null ) ? Convert.ToInt32( analogTag.ReadOnly ) : ( int? ) null,
                                    ArraySize = analogTag.ArraySize,
                                    SpanHigh = analogTag.SpanHigh,
                                    SpanLow = analogTag.SpanLow,
                                    EngineerUnit = analogTag.EngineerUnit,
                                    FractionDisplayFormat = analogTag.FractionDisplayFormat
                                };

                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();
                                deviceObj.TagList.Add( analogTag.Name, analogTagObject );
                            }
                        }

                        if ( device.DiscreteTagList != null )
                        {
                            foreach ( var discreteTag in device.DiscreteTagList )
                            {
                                ConfigMessage.DiscreteTagObject discreteTagObject = new ConfigMessage.DiscreteTagObject()
                                {
                                    Type = TagType.Discrete,
                                    Description = discreteTag.Description,
                                    ReadOnly = ( discreteTag.ReadOnly != null ) ? Convert.ToInt32( discreteTag.ReadOnly ) : ( int? ) null,
                                    ArraySize = discreteTag.ArraySize,
                                    State0 = discreteTag.State0,
                                    State1 = discreteTag.State1,
                                    State2 = discreteTag.State2,
                                    State3 = discreteTag.State3,
                                    State4 = discreteTag.State4,
                                    State5 = discreteTag.State5,
                                    State6 = discreteTag.State6,
                                    State7 = discreteTag.State7
                                };

                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();
                                deviceObj.TagList.Add( discreteTag.Name, discreteTagObject );
                            }
                        }

                        if ( device.TextTagList != null )
                        {
                            foreach ( var textTag in device.TextTagList )
                            {
                                ConfigMessage.TextTagObject textTagObject = new ConfigMessage.TextTagObject()
                                {
                                    Type = TagType.Text,
                                    Description = textTag.Description,
                                    ReadOnly = Convert.ToInt32( textTag.ReadOnly ),
                                    ArraySize = textTag.ArraySize
                                };

                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();
                                deviceObj.TagList.Add( textTag.Name, textTagObject );
                            }
                        }

                        nodeObj.DeviceList.Add( device.Id, deviceObj );
                    }
                }
                msg.D.NodeList.Add( nodeId, nodeObj );

                payload = JsonConverter.SerializeObject( msg );
            }
            catch ( Exception ex )
            {
                _logger.Error( "Convert Config Payload Error ! " + ex.ToString() );
            }
            return payload;
        }
        
        public static string ConvertDeleteConfig( string nodeId, EdgeConfig config )
        {
            string payload = string.Empty;
            try
            {
                if ( config == null )
                    return payload;

                ConfigMessage msg = new ConfigMessage();
                msg.D.Action = ActionType.Delete;
                msg.D.NodeList = new Dictionary<string, ConfigMessage.NodeObject>();

                ConfigMessage.NodeObject nodeObj = new ConfigMessage.NodeObject();

                if ( config.Node.DeviceList != null )
                {
                    nodeObj.DeviceList = new Dictionary<string, ConfigMessage.DeviceObject>();
                    foreach ( var device in config.Node.DeviceList )
                    {
                        ConfigMessage.DeviceObject deviceObj = new ConfigMessage.DeviceObject();

                        if ( device.AnalogTagList != null )
                        {
                            foreach ( var analogTag in device.AnalogTagList )
                            {
                                ConfigMessage.AnalogTagObject analogTagObject = new ConfigMessage.AnalogTagObject();
                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();

                                deviceObj.TagList.Add( analogTag.Name, analogTagObject );
                            }
                        }

                        if ( device.DiscreteTagList != null )
                        {
                            foreach ( var discreteTag in device.DiscreteTagList )
                            {
                                ConfigMessage.DiscreteTagObject discreteTagObject = new ConfigMessage.DiscreteTagObject();
                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();

                                deviceObj.TagList.Add( discreteTag.Name, discreteTagObject );
                            }
                        }

                        if ( device.TextTagList != null )
                        {
                            foreach ( var textTag in device.TextTagList )
                            {
                                ConfigMessage.TextTagObject textTagObject = new ConfigMessage.TextTagObject();
                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();

                                deviceObj.TagList.Add( textTag.Name, textTagObject );
                            }
                        }

                        nodeObj.DeviceList.Add( device.Id, deviceObj );
                    }
                }
                msg.D.NodeList.Add( nodeId, nodeObj );

                payload = JsonConverter.SerializeObject( msg );
            }
            catch ( Exception ex )
            {
                _logger.Error( "Convert Config Payload Error ! " + ex.ToString() );
            }
            return payload;
        }

        public static bool ConvertData( EdgeData data, ref HashSet<string> payloads )
        {
            try
            {
                if ( data == null )
                    return false;

                // split message by limited count
                int count = 0;
                var list = data.TagList.OrderBy( t => t.DeviceId ).ToList();
                DataMessage msg = null;
                for ( int i = 0; i < list.Count; i++ )
                {
                    var tag = list[i];

                    if ( msg == null )
                        msg = new DataMessage();

                    if ( msg.D.ContainsKey( tag.DeviceId ) == false )
                        msg.D[tag.DeviceId] = new Dictionary<string, object>();
                    
                    ( ( Dictionary<string, object> ) msg.D[tag.DeviceId] ).Add( tag.TagName, tag.Value );
                    count++;

                    if ( count == Limit.DataMaxTagCount || i == list.Count - 1 )
                    {
                        msg.Timestamp = data.Timestamp.ToUniversalTime();
                        payloads.Add( JsonConverter.SerializeObject( msg ) );

                        count = 0;
                        msg = null;
                    }
                }
             
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "Convert Data Payload Error ! " + ex.ToString() );
                return false;
            }
        }

        public static bool ConvertData( HashSet<EdgeData> dataset, ref HashSet<string> payloads )
        {
            try
            {
                if ( dataset == null )
                    return false;

                HashSet<DataMessage> messages = new HashSet<DataMessage>();
                foreach ( var data in dataset )
                {
                    DataMessage msg = new DataMessage();
                    foreach ( var tag in data.TagList )
                    {
                        if ( msg.D.ContainsKey( tag.DeviceId ) == false )
                            msg.D[tag.DeviceId] = new Dictionary<string, object>();

                        ( ( Dictionary<string, object> ) msg.D[tag.DeviceId] ).Add( tag.TagName, tag.Value );
                    }
                    msg.Timestamp = data.Timestamp.ToUniversalTime();
                    messages.Add( msg );
                }
                payloads.Add( JsonConverter.SerializeObject( messages ) );
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "Convert Data Payload Error ! " + ex.ToString() );
                return false;
            }
        }

        public static bool ConvertDeviceStatus( EdgeDeviceStatus deviceStatus, ref string payload )
        {
            try
            {
                if ( deviceStatus == null )
                    return false;

                DeviceStatusMessage msg = new DeviceStatusMessage();
                msg.Timestamp = deviceStatus.Timestamp.ToUniversalTime();
                foreach ( var device in deviceStatus.DeviceList )
                {
                    msg.D.DeviceList.Add( device.Id, ( int ) device.Status );
                }
                payload = JsonConverter.SerializeObject( msg );
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "Convert DeviceStatus Payload Error ! " + ex.ToString() );
                return false;
            }
        }

        public static bool ConvertUpdateData( EdgeData data, bool upsert, ref string payload )
        {
            try
            {
                if ( data == null )
                    return false;

                DataManipulateMessage msg = new DataManipulateMessage();
                DataManipulateActionType action = ( upsert == true ) ? DataManipulateActionType.Upsert : DataManipulateActionType.Update;
                msg.D.Action = action;
                foreach ( var tag in data.TagList )
                {
                    msg.D.TagList.Add( new DataManipulateMessage.TagObject()
                    {
                        DeviceId = tag.DeviceId,
                        TagName = tag.TagName,
                        Value = tag.Value,
                        Timestamp = data.Timestamp.ToUniversalTime()
                } );
                }
                payload = JsonConverter.SerializeObject( msg );
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "Convert Update Data Payload Error ! " + ex.ToString() );
                return false;
            }
        }
    }
}
