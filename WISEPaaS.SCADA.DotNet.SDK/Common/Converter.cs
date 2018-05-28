using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WISEPaaS.SCADA.DotNet.SDK.Model;

namespace WISEPaaS.SCADA.DotNet.SDK
{
    public class Converter
    {
        public Converter()
        { }

        public static bool ConvertCreateOrUpdateConfig( EdgeConfig config, ref string payload, int heartbeat = EdgeAgent.DEAFAULT_HEARTBEAT_INTERVAL )
        {
            try
            {
                if ( config == null )
                    return false;

                ConfigMessage msg = new ConfigMessage();
                msg.D.Action = ActionType.Create;
                msg.D.ScadaList = new Dictionary<string, ConfigMessage.ScadaObject>();

                ConfigMessage.ScadaObject scadaObj = new ConfigMessage.ScadaObject()
                {
                    Id = config.Scada.Id,
                    Name = config.Scada.Name,
                    Description = (config.Scada.Description),
                    PrimaryIP = config.Scada.PrimaryIP,
                    BackupIP = config.Scada.BackupIP,
                    PrimaryPort = config.Scada.PrimaryPort,
                    BackupPort = config.Scada.BackupPort,
                    Heartbeat = heartbeat / 1000,
                    Type = SCADAConfigType.SCADA
                };

                if ( config.Scada.DeviceList != null )
                {
                    scadaObj.DeviceList = new Dictionary<string, ConfigMessage.DeviceObject>();
                    foreach ( var device in config.Scada.DeviceList )
                    {
                        ConfigMessage.DeviceObject deviceObj = new ConfigMessage.DeviceObject()
                        {
                            Name = device.Name,
                            Type = device.Type,
                            Description = device.Description,
                            IP = device.IP,
                            Port = device.Port,
                            ComPortNumber = device.ComPortNumber
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
                                    AlarmStatus = ( analogTag.AlarmStatus != null ) ? Convert.ToInt32( analogTag.AlarmStatus ) : ( int? ) null,
                                    NeedLog = ( analogTag.NeedLog != null ) ? Convert.ToInt32( analogTag.NeedLog ) : ( int? ) null,
                                    SpanHigh = analogTag.SpanHigh,
                                    SpanLow = analogTag.SpanLow,
                                    EngineerUnit = analogTag.EngineerUnit,
                                    IntegerDisplayFormat = analogTag.IntegerDisplayFormat,
                                    FractionDisplayFormat = analogTag.FractionDisplayFormat
                                };
                                if ( analogTag.AlarmStatus == true )
                                {
                                    analogTagObject.HHPriority = analogTag.HHPriority;
                                    analogTagObject.HHAlarmLimit = analogTag.HHAlarmLimit;
                                    analogTagObject.HighPriority = analogTag.HighPriority;
                                    analogTagObject.HighAlarmLimit = analogTag.HighAlarmLimit;
                                    analogTagObject.LowPriority = analogTag.LowPriority;
                                    analogTagObject.LowAlarmLimit = analogTag.LowAlarmLimit;
                                    analogTagObject.LLPriority = analogTag.LLPriority;
                                    analogTagObject.LLAlarmLimit = analogTag.LLAlarmLimit;
                                }

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
                                    AlarmStatus = ( discreteTag.AlarmStatus != null ) ? Convert.ToInt32( discreteTag.AlarmStatus ) : ( int? ) null,
                                    NeedLog = ( discreteTag.NeedLog != null ) ? Convert.ToInt32( discreteTag.NeedLog ) : ( int? ) null,
                                    State0 = discreteTag.State0,
                                    State1 = discreteTag.State1,
                                    State2 = discreteTag.State2,
                                    State3 = discreteTag.State3,
                                    State4 = discreteTag.State4,
                                    State5 = discreteTag.State5,
                                    State6 = discreteTag.State6,
                                    State7 = discreteTag.State7
                                };
                                if ( discreteTag.AlarmStatus == true )
                                {
                                    discreteTagObject.State0AlarmPriority = discreteTag.State0AlarmPriority;
                                    discreteTagObject.State1AlarmPriority = discreteTag.State1AlarmPriority;
                                    discreteTagObject.State2AlarmPriority = discreteTag.State2AlarmPriority;
                                    discreteTagObject.State3AlarmPriority = discreteTag.State3AlarmPriority;
                                    discreteTagObject.State4AlarmPriority = discreteTag.State4AlarmPriority;
                                    discreteTagObject.State5AlarmPriority = discreteTag.State5AlarmPriority;
                                    discreteTagObject.State6AlarmPriority = discreteTag.State6AlarmPriority;
                                    discreteTagObject.State7AlarmPriority = discreteTag.State7AlarmPriority;
                                }

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
                                    ArraySize = textTag.ArraySize,
                                    AlarmStatus = Convert.ToInt32( textTag.AlarmStatus ),
                                    NeedLog = Convert.ToInt32( textTag.NeedLog )
                                };

                                if ( deviceObj.TagList == null )
                                    deviceObj.TagList = new Dictionary<string, ConfigMessage.TagObject>();
                                deviceObj.TagList.Add( textTag.Name, textTagObject );
                            }
                        }

                        scadaObj.DeviceList.Add( device.Id, deviceObj );
                    }
                }
                msg.D.ScadaList.Add( config.Scada.Id, scadaObj );

                payload = JsonConvert.SerializeObject( msg, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }


        public static bool ConvertDeleteConfig( EdgeConfig config, ref string payload )
        {
            try
            {
                if ( config == null )
                    return false;

                ConfigMessage msg = new ConfigMessage();
                msg.D.Action = ActionType.Delete;
                msg.D.ScadaList = new Dictionary<string, ConfigMessage.ScadaObject>();

                ConfigMessage.ScadaObject scadaObj = new ConfigMessage.ScadaObject();

                if ( config.Scada.DeviceList != null )
                {
                    scadaObj.DeviceList = new Dictionary<string, ConfigMessage.DeviceObject>();
                    foreach ( var device in config.Scada.DeviceList )
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

                        scadaObj.DeviceList.Add( device.Id, deviceObj );
                    }
                }
                msg.D.ScadaList.Add( config.Scada.Id, scadaObj );

                payload = JsonConvert.SerializeObject( msg, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore } );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        public static bool ConvertData( EdgeData data, ref string payload )
        {
            try
            {
                if ( data == null )
                    return false;

                DataMessage msg = new DataMessage();
                msg.Timestamp = data.Timestamp.ToUniversalTime();
                foreach ( var device in data.DeviceList )
                {
                    Dictionary<string, object> tags = new Dictionary<string, object>();
                    foreach ( var tag in device.TagList )
                    {
                        tags.Add( tag.Name, tag.Value );
                    }
                    msg.D.Add( device.Id, tags );
                }

                payload = JsonConvert.SerializeObject( msg );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
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
                payload = JsonConvert.SerializeObject( msg );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }
    }
}
