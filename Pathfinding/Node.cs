using System.Collections.Generic;
using System.Numerics;

namespace SPToolkits.Pathfinding
{
    public class Node
    {
        public Vector3 position;
        public List<Node> neighbors = new List<Node>();
    }
}