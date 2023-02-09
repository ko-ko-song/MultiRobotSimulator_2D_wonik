using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckCancelMove : AckMessage
{
    public int robotID;
    public AckCancelMove(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int) MessageTypeEnum.MessageType.AckCancelMove;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
