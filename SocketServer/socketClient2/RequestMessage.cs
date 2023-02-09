using System.Collections;
using System.Collections.Generic;
namespace socketClient2
{
    public abstract class RequestMessage
    {
        protected int messageType;

        public int getType()
        {
            return this.messageType;
        }
    }
}