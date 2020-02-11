using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class DataModel
    {
        [Column( "id" )]
        public int Id { get; set; }
        [Column( "message" )]
        public string Message { get; set; }
    }
}
