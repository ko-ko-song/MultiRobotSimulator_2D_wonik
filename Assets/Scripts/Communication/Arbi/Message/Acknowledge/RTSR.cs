using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSR : AckMessage
{
    private int robotID;
    private int status;
    private float posX;
    private float posY;
    private float theta;
    private float speed;
    private int battery;
    private bool loading;

    public RTSR(int robotID, int status, float posX, float posY, float theta,float speed, int battery, bool loading)
    {
        this.robotID = robotID;
        this.status = status;
        this.posX = posX;
        this.posY = posY;
        this.theta = theta;
        this.speed = speed;
        this.battery = battery;
        this.loading = loading;
        this.messageType = (int)MessageTypeEnum.MessageType.RTSR;
    }

    public int getRobotID()
    {
        return this.robotID;
    }
    public int getStatus()
    {
        return this.status;
    }
    public float getPosX()
    {
        return this.posX;
    }
    public float getPosY()
    {
        return this.posY;
    }
    public float getTheta()
    {
        return this.theta;
    }
    public float getSpeed()
    {
        return this.speed;
    }
    public int getBattery()
    {
        return this.battery;
    }
    public bool getLoading()
    {
        return this.loading;
    }


}
