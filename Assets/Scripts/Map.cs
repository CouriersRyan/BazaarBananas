using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DelaunatorSharp;
using DelaunatorSharp.Unity;
using DelaunatorSharp.Unity.Extensions;
using UnityEngine;
using UnityEngine.Android;

// Generates a map of interactable and connected nodes using Delaunay triangulation.
// Uses the DelaunatorSharp Package by nol1fe. https://github.com/nol1fe/delaunator-sharp
// Documentation of Delaunay Triangulation and original project the above Package was adapted from
// https://github.com/mapbox/delaunator by mapbox.
// Inspiration to use A* star with the above package was inspired by yurkth. https://github.com/yurkth/stsmapgen
public class Map : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;

    private List<Node> _nodes = new List<Node>(); // A list of all nodes in the map.
    private Node _startNode;
    private Node _endNode;
    public Node StartNode
    {
        get { return _startNode; }
    }
    public Node EndNode
    {
        get { return _endNode; }
    }
    
    private Delaunator _delaunator;

    //Empty gameobjects that act as the parents of the points and edges generated.
    private Transform _pointsContainer;
    private Transform _trianglesContainer;

    
    [Header("Generation")] [SerializeField]
    
    //Poisson Disk Sample Generation
    private float generationSize = 10; //Radius of the generation
    [SerializeField] private float generationMinDistance = 1f; //Smallest distance between nodes
    
    //Class that handles AStar Algorithm
    private AStar _pathfinding;
    [SerializeField] private int numOfPaths = 4;

    [Header("Lines")]
    [SerializeField] private float edgeWidth = 0.2f;
    [SerializeField] private Color edgeColor = Color.black;
    [SerializeField] protected Material lineMaterial;
    
    

    void Awake()
    {
        GenerateMap();
        _startNode = FindSouthmostNode();
        _endNode = FindNorthmostNode();
        _pathfinding = new AStar();
        CreatePaths(_pathfinding.FindPaths(_startNode, _endNode, _nodes, numOfPaths));
    }


    /*
     * The following code is modified from the DelaunatorSharp Package
     * DelaunatorPreview.cs
     * Most notable changes are that unnecessary functions have been removed, the following functions now only
     * generate a points and triangles (no hull, mesh, voronoi points etc.) and there is not option to toggle.
     * Any comments in here where added by me, Ryan Zhang using the Delaunator project by mapbox as the original
     * package did not have comments.
     *
     * Delaunay triangulation: https://en.wikipedia.org/wiki/Delaunay_triangulation
     * Creates triangles from points that aims to minimize the potential of sliver triangles or triangles that are too
     * long and thin.
     *
     * Poisson Disk Sampling aims to randomly generate points within an area such that all points maintain a specified
     * minimum distance and no closer.
     * 
     */
    
    //Creates a map of connected nodes.
    private void GenerateMap()
    {
        // Poisson sampler to create points.
        // Turn points into a list for deluanator.
        // Create triangles from the points.
        // And generate lines.

        ClearMap();

        var sampler = 
            UniformPoissonDiskSampler.SampleCircle(Vector2.zero, generationSize, generationMinDistance);
        _nodes = sampler.Select(point => new Vector2(point.x, point.y)).ToNodes().ToList();
        Debug.Log($"Generated Points Count {_nodes.Count}");

        CreateDelaunay();
    }
    
    // Triangulates a list of nodes and minimize acute angles.
    private void CreateDelaunay()
    {
        if(_nodes.Count < 3) return; //Cannot create a triangle if there are less than 3 points.
        
        ClearMap();

        _delaunator = new Delaunator(_nodes.OfType<IPoint>().ToArray());

        CreateTriangle();

    }

    // Creates a series of triangles given points inside the delaunator.
    private void CreateTriangle()
    {
        if (_delaunator == null) return;

        _delaunator.ForEachTriangleEdge(edge =>
        {
            float TOLERANCE = 0.01f;
            // Address to equivalent nodes in the _nodes list. *New code*
            var p = _nodes.Find(node => Math.Abs(node.X - edge.P.X) < TOLERANCE && Math.Abs(node.Y - edge.P.Y) < TOLERANCE);
            var q = _nodes.Find(node => Math.Abs(node.X - edge.Q.X) < TOLERANCE && Math.Abs(node.Y - edge.Q.Y) < TOLERANCE);
            var index = _nodes.FindIndex(node => Math.Abs(node.X - edge.P.X) < TOLERANCE && Math.Abs(node.Y - edge.P.Y) < TOLERANCE);
            
            // Creates links to each other. *New code*
            if(!p.Links.Contains(q)) p.Links.Add(q);
            if(!q.Links.Contains(p)) q.Links.Add(p);
        });
    }
    
    // Instantiates a GameObject that draws a line given two points and puts it in a container.
    private void CreateLine(Transform container, string name, Vector3[] points, Color color, float width, int order = 1)
    {
        var lineGameObject = new GameObject(name);
        lineGameObject.transform.parent = container;
        var lineRenderer = lineGameObject.AddComponent<LineRenderer>();

        lineRenderer.SetPositions(points);

        lineRenderer.material = lineMaterial ?? new Material(Shader.Find("Standard")); //TODO figure out what this is.
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        lineRenderer.sortingOrder = order;
    }
    
    // Clears containers and delaunator, but not list of points.
    private void ClearMap()
    {
        CreateNewContainers();

        _delaunator = null;
    }

    private void CreateNewContainers()
    {
        // Clears the points container
        if (_pointsContainer != null)
        {
            Destroy(_pointsContainer.gameObject);
        }

        _pointsContainer = new GameObject(nameof(_pointsContainer)).transform;
        
        // Clears the triangles container
        if (_trianglesContainer != null)
        {
            Destroy(_trianglesContainer.gameObject);
        }

        _trianglesContainer = new GameObject(nameof(_trianglesContainer)).transform;
    }

    private Node FindSouthmostNode()
    {
        var southmostNode = _nodes[0];
        foreach (var node in _nodes)
        {
            if (node.Y < southmostNode.Y)
            {
                southmostNode = node;
            }
        }

        return southmostNode;
    }
    
    private Node FindNorthmostNode()
    {
        var northmostNode = _nodes[0];
        foreach (var node in _nodes)
        {
            if (node.Y > northmostNode.Y)
            {
                northmostNode = node;
            }
        }

        return northmostNode;
    }
    
    //Instantiates gameObjects to represent paths from the list of paths given.
    private void CreatePaths(List<List<Node>> paths)
    {
        //Clear all existing links to prepare to reconnect them with the list of paths.
        foreach (var node in _nodes)
        {
            node.Links.Clear();
        }
        
        
        foreach (var path in paths)
        {
            Node prevNode = null;
            foreach (var node in path)
            {
                if (node.Obj == null)
                {
                    // Instantiates nodes on the points generated.
                    var pointGameObject = Instantiate(nodePrefab, _pointsContainer);
                    pointGameObject.transform.SetPositionAndRotation(node.ToVector3(), Quaternion.identity);
                    node.Obj = pointGameObject; //Makes node aware of the gameObject.
                    pointGameObject.GetComponent<MapNode>().SetNode(node);
                }
                
                if (prevNode != null)
                {
                    // Draws lines between nodes on a path.
                    CreateLine(_trianglesContainer, "Path", new Vector3[] { prevNode.ToVector3(), node.ToVector3() }, edgeColor, edgeWidth, 0);
                    
                    // Previously, links to other nodes were two-way, now that paths have been established we redefine
                    // the links as one way.
                    prevNode.Links.Add(node); // Adds the current node as a link of the previous Node
                }
                prevNode = node;
            }
        }
    }
}
