using System.Collections;
using System.Collections.Generic;

namespace socketClient3
{
    public class RequestCancelMove : RequestMessage
    {
        private int robotID;

        public RequestCancelMove(int robotID)
        {
            this.messageType = (int)MessageTypeEnum.MessageType.ReqCancelMove;
            this.robotID = robotID;
        }

        public int getRobotID()
        {
            return this.robotID;
        }


    }
}