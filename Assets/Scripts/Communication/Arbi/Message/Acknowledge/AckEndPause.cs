using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndPause : AckMessage
{
    public int robotID;
    public AckEndPause(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndPause;
    }

    public int getRobotID()
    {
        return this.robotID;
    }

}
