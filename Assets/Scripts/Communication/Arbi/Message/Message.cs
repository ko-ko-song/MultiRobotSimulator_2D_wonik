using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ArbiMessage 
{
    protected int messageType;

    public int getType()
    {
        return this.messageType;
    }
}
