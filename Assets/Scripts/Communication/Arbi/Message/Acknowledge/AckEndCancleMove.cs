using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndCancleMove : AckMessage
{
    public int robotID;
    public int result;
    public AckEndCancleMove(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndCancelMove;
    }

    public int getRobotID()
    {
        return this.robotID;
    }

    public int getResult()
    {
        return this.result;
    }
}
