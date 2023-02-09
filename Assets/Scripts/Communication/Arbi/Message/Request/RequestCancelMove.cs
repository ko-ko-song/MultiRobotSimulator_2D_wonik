using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
