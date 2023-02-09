using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonCall : AckMessage
{
    private int locationID;
    private int callID;

    public PersonCall(int locationID, int callID)
    {
        this.locationID = locationID;
        this.callID = callID;
        this.messageType = (int)MessageTypeEnum.MessageType.AckPersonCall;
    }

    public int getLocationID()
    {
        return this.locationID;
    }

    public int getCallID()
    {
        return this.callID;
    }
}
