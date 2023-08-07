using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm 
{
    public List<string> FindPath(Dictionary<string, AStarNode> graph, string startId, string goalId)
    {
        Dictionary<string, string> cameFrom = new Dictionary<string, string>();
        Dictionary<string, float> costSoFar = new Dictionary<string, float>();

        PriorityQueue<string> frontier = new PriorityQueue<string>();
        frontier.Enqueue(0, startId);

        cameFrom[startId] = null;
        costSoFar[startId] = 0;

        while (frontier.Count() > 0)
        {
            string currentId = frontier.Dequeue();

            if (currentId == goalId)
                break;

            AStarNode currentNode = graph[currentId];
            
            foreach (string nextId in currentNode.edges)
            {
                float newCost = costSoFar[currentId] + 1; // Assuming edge cost is 1

                if (!costSoFar.ContainsKey(nextId) || newCost < costSoFar[nextId])
                {
                    costSoFar[nextId] = newCost;
                    float priority = newCost + Heuristic(graph[nextId].position, graph[goalId].position);
                    frontier.Enqueue(priority, nextId);
                    cameFrom[nextId] = currentId;
                }
            }
        }


        return ReconstructPath(cameFrom, startId, goalId);
    }

    private float Heuristic(Vector2 positionA, Vector2 positionB)
    {
        return Vector2.Distance(positionA, positionB);
    }

    private List<string> ReconstructPath(Dictionary<string, string> cameFrom, string startId, string goalId)
    {
        
        try
        {
            List<string> path = new List<string>();
            string currentId = goalId;

            while (currentId != startId)
            {
                path.Insert(0, currentId);
                Debug.Log(currentId);
                currentId = cameFrom[currentId];
            }

            path.Insert(0, startId);
            return path;
        }
        catch
        {
            List<string> path = new List<string>();
            return path;
        }
        

    }
}

