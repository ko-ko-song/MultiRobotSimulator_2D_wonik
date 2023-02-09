using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckDoorClose : AckMessage
{
    public int robotID;
    public AckDoorClose(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckDoorClose;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
