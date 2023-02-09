using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndDoorClose : AckMessage
{
    public int robotID;
    public int result;
    public AckEndDoorClose(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndDoorClose;
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
