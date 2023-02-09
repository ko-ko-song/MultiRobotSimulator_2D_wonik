using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckResume : AckMessage
{
    public int robotID;
    public AckResume(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckResume;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
