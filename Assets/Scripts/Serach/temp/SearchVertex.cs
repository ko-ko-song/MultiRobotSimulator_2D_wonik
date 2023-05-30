using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchVertex
{
    private SearchVertex        parent;
    private SearchVertex[]      children;
    private Vertex              vertex;
    private float               distanceTraveledFromStartPosition; //현재까지 이동거리
    private float               optimalDistance; //현재까지 이동거리 + 직선거리


    public SearchVertex(SearchVertex parent, Vertex vertex)
    {
        this.parent = parent;
        this.vertex = vertex;
    }

    public Vertex getVertex()
    {
        return this.vertex;
    }
    
    public SearchVertex getParent()
    {
        return this.parent;
    }    

    public void setChildren(SearchVertex[] array)
    {
        this.children = array;
    }

    public SearchVertex[] getChildren()
    {
        return this.children;
    }
    
    public float getDistanceTraveledFromStartPosition()
    {
        return this.distanceTraveledFromStartPosition;
    }
    
    public void setDistanceTraveledFromStartPosition(float distanceFromStartPosition)
    {
        this.distanceTraveledFromStartPosition = distanceFromStartPosition;
    }

    public float getOptimalDistance()
    {
        return this.optimalDistance;

    }

    public void setOptimalDistance(float distance)
    {
        this.optimalDistance = distance;
    }

}
