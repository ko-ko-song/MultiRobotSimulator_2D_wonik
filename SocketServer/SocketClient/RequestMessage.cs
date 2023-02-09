using System.Collections;
using System.Collections.Generic;
namespace socketClient
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