using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestCharge : RequestMessage
{
    private int robotID;
    private int nodeID;

    public RequestCharge(int robotID, int nodeID)
    {
        this.messageType = (int)MessageTypeEnum.MessageType.ReqCharge;
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
