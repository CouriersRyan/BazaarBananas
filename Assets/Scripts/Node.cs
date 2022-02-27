using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using UnityEngine;

// Version of DelaunatorSharp's Point struct that is aware of its connections to other nodes/points.
// In DelaunatorSharp
// See README.md or Map.cs for more details on credits.
public class Node : IPoint
{
    public double X { get; set; }

    public double Y { get; set; }
    
    public Node(double x, double y)
    {
        X = x;
        Y = y;
        Links = new List<Node>();
        _obj = null;
    }

    // List of all other nodes that this node is connected to.
    public List<Node> Links;
    
    // GameObject tied to this node.
    private GameObject _obj;

    public GameObject Obj
    {
        get { return _obj; }
        set { _obj = value; }
    }
    
    // A* Variables
    private float _gCost; //Cost of moving to this node from the start.
    private float _hCost; //Heuristic cost of this node to the end.
    public float GCost { get; set; }
    public float HCost { get; set; }
    
    // Combined g and h costs. Total cost of the node.
    public float FCost 
    {
        get { return GCost + HCost; }
    }

    public Node PrevNode; // The node that precedes this one in the path.
    public bool isExcluded = false; // Whether or not to exclude this node from pathfinding.

    public override string ToString() => string.Format("{0},{1}", (object) this.X, (object) this.Y);
}

// Static class containing same functions as DelaunatorExtensions class in DelaunatorSharp.Unity.Extensions, except
// Modified to use to above struct of Nodes instead of Points.
public static class DelaunatorNodes
{
    public static Node[] ToNodes(this IEnumerable<Vector2> vertices) => vertices.Select(vertex => new Node(vertex.x, vertex.y)).ToArray();
    public static Node[] ToNodes(this Transform[] vertices) => vertices.Select(x => x.transform.position).OfType<Vector2>().ToNodes();

}
