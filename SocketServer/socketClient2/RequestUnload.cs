using System.Collections;
using System.Collections.Generic;
namespace socketClient2
{
    public class RequestUnload : RequestMessage
    {
        private int robotID;
        private int nodeID;

        public RequestUnload(int robotID, int nodeID)
        {
            this.messageType = (int)MessageTypeEnum.MessageType.ReqUnload;
            this.robotID = robotID;
            this.nodeID = nodeID;
        }
        public int getRobotID()
        {
            return this.robotID;
        }
        public int getNodeID()
        {
            return this.nodeID;
        }
    }
}