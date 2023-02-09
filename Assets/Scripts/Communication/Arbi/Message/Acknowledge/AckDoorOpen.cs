using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckDoorOpen : AckMessage
{
    public int robotID;
    public AckDoorOpen(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckDoorOpen;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
