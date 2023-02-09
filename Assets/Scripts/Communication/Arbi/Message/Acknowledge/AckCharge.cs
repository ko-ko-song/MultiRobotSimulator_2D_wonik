using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckCharge : AckMessage
{
    public int robotID;
    public AckCharge(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckCharge;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
