using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndCharge : AckMessage
{
    public int robotID;
    public int result;
    public AckEndCharge(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndCharge;
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
