using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndDoorOpen : AckMessage
{
    public int robotID;
    public int result;
    public AckEndDoorOpen(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndDoorOpen;
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
