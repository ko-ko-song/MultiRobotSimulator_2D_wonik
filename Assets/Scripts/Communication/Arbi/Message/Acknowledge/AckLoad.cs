using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AckLoad : AckMessage
{
    public int robotID;
    public AckLoad(int robotID)
    {
        this.robotID = robotID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckLoad;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
}
