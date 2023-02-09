using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndResume : AckMessage
{
    public int robotID;
    public AckEndResume(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndResume;
    }

    public int getRobotID()
    {
        return this.robotID;
    }

}
