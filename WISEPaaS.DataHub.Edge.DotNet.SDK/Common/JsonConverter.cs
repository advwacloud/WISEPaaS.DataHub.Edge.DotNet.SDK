using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK
{
    public static class JsonConverter
    {
        public static string SerializeObject( object value )
        {
            return JsonConvert.SerializeObject( value, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            } );
        }

        public static T DeserializeObject<T>( string message )
        {
            return JsonConvert.DeserializeObject<T>( message );
        }
    }
}
