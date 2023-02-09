using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndChargeStop : AckMessage
{
    public int robotID;
    public int result;
    public AckEndChargeStop(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndChargeStop;
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
