using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{

    private Dictionary<string, AStarNode> nodes= new Dictionary<string, AStarNode>();
    private AStarAlgorithm algorithm;
    public Graph()
    {
        algorithm = new AStarAlgorithm();
        Init();
    }

    private void Init()
    {
        // Create nodes and initialize the node map
        foreach (EnvironmentObject vertex in EnvironmentManager.instance.vertexes.Values)
        {
            AStarNode node = new AStarNode(vertex.id, vertex.position);
            nodes.Add(vertex.id, node);
        }
        
        // Add edges to the corresponding nodes
        foreach (Edge edge in EnvironmentManager.instance.edges)
        {
            if (nodes.ContainsKey(edge.vertex1Id) && nodes.ContainsKey(edge.vertex2Id))
            {
                AStarNode node1 = nodes[edge.vertex1Id];
                AStarNode node2 = nodes[edge.vertex2Id];

                node1.edges.Add(node2.id);
                node2.edges.Add(node1.id);
            }
        }

    }

    public List<string> FindPah(string start, string goal)
    {
        return algorithm.FindPath(nodes, start, goal);
    }
    
}
