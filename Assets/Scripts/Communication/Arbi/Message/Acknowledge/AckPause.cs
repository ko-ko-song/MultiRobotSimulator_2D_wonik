using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckPause : AckMessage
{
    public int robotID;
    public AckPause(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckPause;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
