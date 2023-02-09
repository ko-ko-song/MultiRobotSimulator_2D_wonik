using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndMove : AckMessage
{
    public int robotID;
    public int result;
    public AckEndMove(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndMove;
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
