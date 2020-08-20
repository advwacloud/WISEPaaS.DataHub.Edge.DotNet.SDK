using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        private Dictionary<string, ConfigMessage.DeviceObject> _configCache;

        public ConfigCacheHelper()
        {
            _configCache = new Dictionary<string, ConfigMessage.DeviceObject>();
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
                    _configCache = JsonConverter.DeserializeObject<Dictionary<string, ConfigMessage.DeviceObject>>( file.ReadToEnd() );
                }
                return true;
            }
            catch
            {
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
            catch
            {
                return false;
            }
        }

        public bool InsertConfigCache( ConfigMessage message )
        {
            try
            {
                if ( message == null || message.D == null || message.D.NodeList.Count == 0 )
                    return false;

                string nodeId = message.D.NodeList.Keys.First();
                _configCache = message.D.NodeList[nodeId].DeviceList;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpsertConfigCache( ConfigMessage message )
        {
            try
            {
                if ( message == null || message.D == null || message.D.NodeList.Count == 0 )
                    return false;

                string nodeId = message.D.NodeList.Keys.First();

                JObject o1 = JObject.FromObject( _configCache );
                JObject o2 = JObject.FromObject( message.D.NodeList[nodeId].DeviceList );

                o1.Merge( o2, new JsonMergeSettings
                {
                    MergeNullValueHandling = MergeNullValueHandling.Ignore,
                    MergeArrayHandling = MergeArrayHandling.Union
                } );

                var list = JsonConvert.DeserializeObject<Dictionary<string, ConfigMessage.DeviceObject>>( o1.ToString(), new ConfigMessage.TagObjectConverter() );
                _configCache = list;
                return true;
            }
            catch
            {
                return false;
            }

        }

        public bool DeleteConfigCache( ConfigMessage message )
        {
            try
            {
                if ( message == null || message.D == null || message.D.NodeList.Count == 0 )
                    return false;

                string nodeId = message.D.NodeList.Keys.First();
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
                            _configCache[deviceId].TagList.Remove( tagName );
                        }
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
