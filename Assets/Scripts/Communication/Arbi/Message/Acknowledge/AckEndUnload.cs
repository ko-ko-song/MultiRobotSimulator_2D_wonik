using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckEndUnload : AckMessage
{
    public int robotID;
    public int result;
    public AckEndUnload(int robotID, int result)
    {
        this.robotID = robotID;
        this.result = result;
        this.messageType = (int)MessageTypeEnum.MessageType.AckEndUnload;
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
