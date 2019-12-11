using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WISEPaaS.SCADA.DotNet.SDK
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
    }
}
