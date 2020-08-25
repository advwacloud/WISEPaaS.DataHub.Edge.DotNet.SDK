using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WISEPaaS.DataHub.Edge.DotNet.SDK.Model;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK
{
    public class ConfigCacheHelper
    {
        private const string configFileName = "config.json";

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        
        //private dynamic _configCache = new ExpandoObject() as IDictionary<string, Object>;

        private Dictionary<string, Dictionary<string, ConfigCache.TagObject>> _configCache;

        public ConfigCacheHelper()
        {
            _configCache = new Dictionary<string, Dictionary<string, ConfigCache.TagObject>>();
        }

        public bool LoadConfigFromFile()
        {
            try
            {
                string path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, configFileName );
                if ( !File.Exists( path ) )
                    return false;

                using ( StreamReader file = File.OpenText( path ) )
                {
                    _configCache = JsonConverter.DeserializeObject<Dictionary<string, Dictionary<string, ConfigCache.TagObject>>>( file.ReadToEnd() );
                    //_configCache = JObject.Parse( file.ReadToEnd() );
                }
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "LoadConfigFromFile Error ! " + ex.ToString() );
                return false;
            }
        }

        public bool SaveConfigToFile()
        {
            try
            {
                string path = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, configFileName );
                using ( FileStream stream = new FileStream( path, FileMode.Create ) )
                using ( StreamWriter writer = new StreamWriter( stream, Encoding.UTF8 ) )
                {
                    string context = JsonConverter.SerializeObject( _configCache );
                    writer.Write( context );
                }
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "SaveConfigToFile Error ! " + ex.ToString() );
                return false;
            }
        }

        public bool InsertConfigCache( EdgeConfig config )
        {
            try
            {
                if ( config == null )
                    return false;

                //string nodeId = message.D.NodeList.Keys.First();
                //_configCache = message.D.NodeList[nodeId].DeviceList;
                /*_configCache = new ExpandoObject();
                foreach ( var deviceKeyPair in message.D.NodeList[nodeId].DeviceList )
                {
                    string deviceId = deviceKeyPair.Key;
                    (_configCache as IDictionary<string, Object>).Add( deviceId, new ExpandoObject() );
                    foreach ( var tagKeyPair in deviceKeyPair.Value.TagList )
                    {
                        string tagName = tagKeyPair.Key;
                        ( _configCache as IDictionary<string, Object> )[deviceId].Add( tagName, tagKeyPair.Value );
                    }
                }*/
                _configCache = new Dictionary<string, Dictionary<string, ConfigCache.TagObject>>();
                foreach ( var device in config.Node.DeviceList )
                {
                    _configCache.Add( device.Id, new Dictionary<string, ConfigCache.TagObject>() );
                    foreach ( var analogTag in device.AnalogTagList )
                    {
                        _configCache[device.Id].Add( analogTag.Name, new ConfigCache.AnalogTagObject()
                        {
                            SendWhenValueChanged = analogTag.SendWhenValueChanged,
                            SpanHigh = analogTag.SpanHigh,
                            SpanLow = analogTag.SpanLow,
                            IntegerDisplayFormat = analogTag.IntegerDisplayFormat,
                            FractionDisplayFormat = analogTag.FractionDisplayFormat
                        } );
                    }
                }

                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "InsertConfigCache Error ! " + ex.ToString() );
                return false;
            }
        }

        public bool UpsertConfigCache( EdgeConfig config )
        {
            try
            {
                if ( config == null )
                    return false;

                /*string nodeId = message.D.NodeList.Keys.First();

                JObject o1 = JObject.FromObject( _configCache );
                JObject o2 = JObject.FromObject( message.D.NodeList[nodeId].DeviceList );

                o1.Merge( o2, new JsonMergeSettings
                {
                    MergeNullValueHandling = MergeNullValueHandling.Ignore,
                    MergeArrayHandling = MergeArrayHandling.Union
                } );

                var list = JsonConvert.DeserializeObject<Dictionary<string, ConfigMessage.DeviceObject>>( o1.ToString(), new ConfigMessage.TagObjectConverter() );
                _configCache = list;*/
                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "UpsertConfigCache Error ! " + ex.ToString() );
                return false;
            }

        }

        public bool DeleteConfigCache( EdgeConfig config )
        {
            try
            {
                if ( config == null )
                    return false;

                /*string nodeId = message.D.NodeList.Keys.First();
                foreach ( KeyValuePair<string, ConfigMessage.DeviceObject> deviceKeyPair in message.D.NodeList[nodeId].DeviceList )
                {
                    string deviceId = deviceKeyPair.Key;
                    if ( deviceKeyPair.Value == null || deviceKeyPair.Value.TagList == null || deviceKeyPair.Value.TagList.Count == 0 )
                    {
                        _configCache.Remove( deviceId );
                    }
                    else
                    {
                        foreach ( KeyValuePair<string, ConfigMessage.TagObject> tagKeyPair in deviceKeyPair.Value.TagList )
                        {
                            string tagName = tagKeyPair.Key;
                            _configCache[deviceId].Remove( tagName );
                        }
                    }
                }*/

                return true;
            }
            catch ( Exception ex )
            {
                _logger.Error( "DeleteConfigCache Error ! " + ex.ToString() );
                return false;
            }
        }
    }
}
