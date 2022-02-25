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

    public override string ToString() => string.Format("{0},{1}", (object) this.X, (object) this.Y);
}

// Static class containing same functions as DelaunatorExtensions class in DelaunatorSharp.Unity.Extensions, except
// Modified to use to above struct of Nodes instead of Points.
public static class DelaunatorNodes
{
    public static Node[] ToNodes(this IEnumerable<Vector2> vertices) => vertices.Select(vertex => new Node(vertex.x, vertex.y)).ToArray();
    public static Node[] ToNodes(this Transform[] vertices) => vertices.Select(x => x.transform.position).OfType<Vector2>().ToNodes();

}
