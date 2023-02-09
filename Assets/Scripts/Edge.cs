using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
    public string id;
    public string vertex1Id;
    public string vertex2Id;

    public Edge(string id, string vertex1Id, string vertex2Id)
    {
        this.id = id;
        this.vertex1Id = vertex1Id;
        this.vertex2Id = vertex2Id;
    }

}
