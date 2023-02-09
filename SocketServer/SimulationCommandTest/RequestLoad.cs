using System.Collections;
using System.Collections.Generic;
namespace socketClient
{
    public class RequestLoad : RequestMessage
    {
        private int robotID;
        private int nodeID;

        public RequestLoad(int robotID, int nodeID)
        {
            this.messageType = (int)MessageTypeEnum.MessageType.ReqLoad;
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