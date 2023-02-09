using System.Collections;
using System.Collections.Generic;
namespace socketClient2
{
    public class RequestDoorOpen : RequestMessage
    {
        private int doorID;

        public RequestDoorOpen(int doorID)
        {
            this.messageType = (int)MessageTypeEnum.MessageType.ReqDoorOpen;
            this.doorID = doorID;
        }

        public int getDoorID()
        {
            return this.doorID;
        }
    }
}