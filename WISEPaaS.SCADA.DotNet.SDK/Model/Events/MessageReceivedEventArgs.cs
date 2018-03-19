using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WISEPaaS.SCADA.DotNet.SDK.Model
{
    public class MessageReceivedEventArgs : EventArgs
    {
        private MessageType _type;
        private BaseMessage _message;

        public MessageType Type
        {
            get
            {
                return _type;
            }
        }

        public BaseMessage Message
        {
            get
            {
                return _message;
            }
        }

        public MessageReceivedEventArgs( MessageType type, BaseMessage message )
        {
            _type = type;
            _message = message;
        }
    }
}
