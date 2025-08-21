using System.Collections.Generic;
using UnityEngine;

namespace SPToolkits.Pathfinding
{ 
    public static class Pathfinder
    {
        public static Node[] GeneratePath(Node start, Node end, List<Node> graph) 
        {
            Node[] path = new Node[graph.Count];
            for (int i = 0; i < path.Length; i++) 
            {
                path[i] = graph[i];
            }
            return path;
        }
    }
}