using System.Collections;
using System.Collections.Generic;
namespace socketClient3
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