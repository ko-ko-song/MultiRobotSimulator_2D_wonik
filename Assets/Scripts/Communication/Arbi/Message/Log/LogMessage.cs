using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogMessage
{
    private string objectID;
    private string content;
    private float locationX;
    private float locationY;
    private float targetLocationX;
    private float targetLocationY;
    
    public LogMessage(string id, string content, float locationX, float locationY, float targetLocationX, float targetLocationY)
    {
        this.objectID = id;
        this.content = content;
        this.locationX = locationX;
        this.locationY = locationY;
        this.targetLocationX = targetLocationX;
        this.targetLocationY = targetLocationY;
    }

    public LogMessage(string id, string content, float locationX, float locationY)
    {
        this.targetLocationX = 9999;
        this.targetLocationY = 9999;
        this.objectID = id;
        this.content = content;
        this.locationX = locationX;
        this.locationY = locationY;
    }

    public string getObjectID()
    {
        return this.objectID;
    }

    public string getContent()
    {
        return this.content;
    }

    public float getLocationX()
    {
        return this.locationX;
    }
    public float getLocationY()
    {
        return this.locationY;
    }
    public float getTargetLocationX()
    {
        return this.targetLocationX;
    }
    public float getTargetLocationY()
    {
        return this.targetLocationY;
    }


}
