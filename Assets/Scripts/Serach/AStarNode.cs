using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public string id;
    public List<string> edges;
    public Vector2 position;

    public AStarNode(string id, Vector2 position)
    {
        this.id = id;
        this.position = position;
        edges = new List<string>();
    }
}
