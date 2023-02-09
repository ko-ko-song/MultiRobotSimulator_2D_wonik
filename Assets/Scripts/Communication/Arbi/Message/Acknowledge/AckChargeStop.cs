using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckChargeStop : AckMessage
{
    public int robotID;
    public AckChargeStop(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckChargeStop;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
