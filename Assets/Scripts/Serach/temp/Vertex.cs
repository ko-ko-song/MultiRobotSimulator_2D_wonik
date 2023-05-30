using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex
{
    public string[] edges;
    public int type;
    public float posX;
    public float posY;
    public string name;
    

    public void setName(string name)
    {
        this.name = name;
    }

    public void setEdges(string[] edges)
    {
        this.edges = edges;
    }

    public void setType(int type)
    {
        this.type = type;
    }
    public void setPosX(float posX)
    {
        this.posX = posX;
    }
    public void setPosY(float posY)
    {
        this.posY = posY;
    }
}

