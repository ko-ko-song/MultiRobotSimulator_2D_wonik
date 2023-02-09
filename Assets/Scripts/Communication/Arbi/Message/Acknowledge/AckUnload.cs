using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckUnload : AckMessage
{
    public int robotID;
    public AckUnload(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckUnload;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
