using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.DataHub.Edge.DotNet.SDK.Model
{
    public class EdgeAgentConnectedEventArgs
    {
        private bool _isSessionPresent;
        public bool IsSessionPresent 
        {
            get 
            {
                return _isSessionPresent;
            }
        }

        public EdgeAgentConnectedEventArgs( bool isSessionPresent )
        {
            _isSessionPresent = isSessionPresent;
        }
    }
}
