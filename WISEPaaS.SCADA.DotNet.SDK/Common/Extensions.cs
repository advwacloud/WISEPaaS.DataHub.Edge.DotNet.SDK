using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK
{
    public static class Extensions
    {
        public static List<T> GetAndRemoveRange<T>( this List<T> list, int index, int count )
        {
            List<T> items = new List<T>();
            if ( index < 0 || index > list.Count )
                return items;

            int length = ( list.Count < count ) ? list.Count : count;
            items = list.GetRange( index, length );
            list.RemoveRange( index, length );
            return items;
        }
    }
}
