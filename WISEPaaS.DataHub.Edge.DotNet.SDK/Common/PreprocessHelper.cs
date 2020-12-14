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
    public class PreprocessHelper
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        private const string configFileNameFormat = "{0}_config.json";
        private string configFileName;

        private Dictionary<string, Dictionary<string, ConfigCache.TagObject>> _configCacheList;
        private Dictionary<string, DataCache> _dataCacheList;
        
        public PreprocessHelper( string nodeId )
        {
            configFileName = string.Format( configFileNameFormat, nodeId );
            _configCacheList = new Dictionary<string, Dictionary<string, ConfigCache.TagObject>>();    // record the config of all tags
            _dataCacheList = new Dictionary<string, DataCache>();    // record the previous data of all tags
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
                    _configCacheList = JsonConverter.DeserializeObject<Dictionary<string, Dictionary<string, ConfigCache.TagObject>>>( file.ReadToEnd() );
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
                    string context = JsonConverter.SerializeObject( _configCacheList );
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

        public void InsertConfigCache( EdgeConfig config )
        {
            try
            {
                if ( config == null )
                    return;

                _configCacheList = new Dictionary<string, Dictionary<string, ConfigCache.TagObject>>();
                foreach ( var device in config.Node.DeviceList )
                {
                    _configCacheList.Add( device.Id, new Dictionary<string, ConfigCache.TagObject>() );
                    foreach ( var tag in device.AnalogTagList )
                    {
                        _configCacheList[device.Id].Add( tag.Name, new ConfigCache.AnalogTagObject()
                        {
                            SendWhenValueChanged = tag.SendWhenValueChanged,
                            SpanHigh = tag.SpanHigh,
                            SpanLow = tag.SpanLow,
                            FractionDisplayFormat = tag.FractionDisplayFormat
                        } );
                    }
                    foreach ( var tag in device.DiscreteTagList )
                    {
                        _configCacheList[device.Id].Add( tag.Name, new ConfigCache.DiscreteTagObject()
                        {
                            SendWhenValueChanged = tag.SendWhenValueChanged
                        } );
                    }
                    foreach ( var tag in device.TextTagList )
                    {
                        _configCacheList[device.Id].Add( tag.Name, new ConfigCache.TextTagObject()
                        {
                            SendWhenValueChanged = tag.SendWhenValueChanged
                        } );
                    }
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "InsertConfigCache Error ! " + ex.ToString() );
            }
        }

        public void UpdateConfigCache( EdgeConfig config )
        {
            try
            {
                if ( config == null )
                    return;

                foreach ( var device in config.Node.DeviceList )
                {
                    if ( _configCacheList.ContainsKey( device.Id ) == false )
                        _configCacheList.Add( device.Id, new Dictionary<string, ConfigCache.TagObject>() );

                    foreach ( var tag in device.AnalogTagList )
                    {
                        var tagObj = new ConfigCache.AnalogTagObject()
                        {
                            SendWhenValueChanged = tag.SendWhenValueChanged,
                            SpanHigh = tag.SpanHigh,
                            SpanLow = tag.SpanLow,
                            FractionDisplayFormat = tag.FractionDisplayFormat
                        };

                        if ( _configCacheList[device.Id].ContainsKey( tag.Name ) == false )
                            _configCacheList[device.Id].Add( tag.Name, tagObj );
                        else
                            _configCacheList[device.Id][tag.Name] = tagObj;
                    }
                    foreach ( var tag in device.DiscreteTagList )
                    {
                        var tagObj = new ConfigCache.DiscreteTagObject()
                        {
                            SendWhenValueChanged = tag.SendWhenValueChanged
                        };

                        if ( _configCacheList[device.Id].ContainsKey( tag.Name ) == false )
                            _configCacheList[device.Id].Add( tag.Name, tagObj );
                        else
                            _configCacheList[device.Id][tag.Name] = tagObj;
                    }
                    foreach ( var tag in device.TextTagList )
                    {
                        var tagObj = new ConfigCache.TagObject()
                        {
                            SendWhenValueChanged = tag.SendWhenValueChanged
                        };

                        if ( _configCacheList[device.Id].ContainsKey( tag.Name ) == false )
                            _configCacheList[device.Id].Add( tag.Name, tagObj );
                        else
                            _configCacheList[device.Id][tag.Name] = tagObj;
                    }
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "UpsertConfigCache Error ! " + ex.ToString() );
            }
        }

        public void DeleteConfigCache( EdgeConfig config )
        {
            try
            {
                if ( config == null )
                    return;

                if ( config.Node.DeviceList == null || config.Node.DeviceList.Count == 0 )
                {
                    _configCacheList = new Dictionary<string, Dictionary<string, ConfigCache.TagObject>>();
                    return;
                }
                foreach ( var device in config.Node.DeviceList )
                {
                    if ( device.AnalogTagList.Count + device.DiscreteTagList.Count + device.TextTagList.Count == 0 )
                    {
                        _configCacheList.Remove( device.Id );
                        continue;
                    }
                    foreach ( var tag in device.AnalogTagList )
                    {
                        _configCacheList[device.Id].Remove( tag.Name );
                    }
                    foreach ( var tag in device.DiscreteTagList )
                    {
                        _configCacheList[device.Id].Remove( tag.Name );
                    }
                    foreach ( var tag in device.TextTagList )
                    {
                        _configCacheList[device.Id].Remove( tag.Name );
                    }
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "DeleteConfigCache Error ! " + ex.ToString() );
            }
        }

        public EdgeData GetAvailEdgeData( EdgeData data )
        {
            EdgeData availData = new EdgeData();
            availData.Timestamp = data.Timestamp;
            try
            {
                foreach ( var tag in data.TagList )
                {
                    // Check tag info is included in config or not. If not, bypass convert process
                    if ( _configCacheList.ContainsKey( tag.DeviceId ) == false ||
                        _configCacheList[tag.DeviceId].ContainsKey( tag.TagName ) == false )
                    {
                        availData.TagList.Add( tag );
                        continue;
                    }

                    if ( _configCacheList[tag.DeviceId][tag.TagName].SendWhenValueChanged == false )
                    {
                        availData.TagList.Add( tag );
                        continue;
                    }

                    // Compare tag current value with the previous value for value changed checking
                    string key = string.Format( "{0}_{1}", tag.DeviceId, tag.TagName );
                    if ( _dataCacheList.ContainsKey( key ) == false )
                    {
                        _dataCacheList.Add( key, new DataCache()
                        {
                            Value = tag.Value,
                            Timestamp = data.Timestamp
                        } );

                        availData.TagList.Add( tag );
                        continue;
                    }

                    var cache = _dataCacheList[key];
                    if ( data.Timestamp > cache.Timestamp && tag.Value.Equals( cache.Value ) == false )
                    {
                        _dataCacheList[key].Value = tag.Value;
                        _dataCacheList[key].Timestamp = data.Timestamp;

                        availData.TagList.Add( tag );
                    }
                }
            }
            catch ( Exception ex )
            {
                _logger.Error( "CheckValueChanged Error ! " + ex.ToString() );
            }

            return availData;
        }
    }
}
