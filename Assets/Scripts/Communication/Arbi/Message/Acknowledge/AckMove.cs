using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckMove : AckMessage
{
    public int robotID;
    public AckMove(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckMove;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
