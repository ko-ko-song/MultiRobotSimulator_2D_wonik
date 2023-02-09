using System.Collections;
using System.Collections.Generic;
namespace socketClient3
{
    public class RequestChargeStop : RequestMessage
    {
        private int robotID;
        private int nodeID;

        public RequestChargeStop(int robotID, int nodeID)
        {
            this.messageType = (int)MessageTypeEnum.MessageType.ReqChargeStop;
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