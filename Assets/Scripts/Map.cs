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
    //TODO Set up A* Alogrithm for path finding.
    //TODO Set up events to run into.
    //TODO Set up player with resources.
    //MVC for player/GUI where it retreaves events to process.

    [SerializeField] private GameObject nodePrefab;

    private List<Node> _nodes = new List<Node>(); // A list of all nodes in the map.

    private Delaunator _delaunator;

    //Empty gameobjects that act as the parents of the points and edges generated.
    private Transform _pointsContainer;
    private Transform _trianglesContainer;

    //Poisson Disk Sample Generation
    [Header("Generation")] [SerializeField]
    private float generationSize = 10; //Radius of the generation

    [SerializeField] private float generationMinDistance = 1f; //Smallest distance between nodes

    [Header("Lines")] [SerializeField] private float edgeWidth = 0.2f;
    [SerializeField] private Color edgeColor = Color.black;
    [SerializeField] protected Material lineMaterial;
    
    void Start()
    {
        GenerateMap();
    }
    
    void Update()
    {
        
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
            // Address to equivalent nodes in the _nodes list. *New code*
            var p = _nodes.Find(node => node.X == edge.P.X && node.Y == edge.P.Y);
            var q = _nodes.Find(node => node.X == edge.Q.X && node.Y == edge.Q.Y);
            
            // Draws lines between points based on the edges outputted by triangulation.
            CreateLine(_trianglesContainer, $"TriangleEdge - {edge.Index}", new Vector3[] { edge.P.ToVector3(), edge.Q.ToVector3() }, edgeColor, edgeWidth, 0);
            
            // Creates links to each other. *New code*
            if(!p.Links.Contains(q)) p.Links.Add(q);
            if(!q.Links.Contains(p)) q.Links.Add(p);
            
            // Instantiates nodes on the points generated.
            var pointGameObject = Instantiate(nodePrefab, _pointsContainer);
                pointGameObject.transform.SetPositionAndRotation(edge.P.ToVector3(), Quaternion.identity);
                p.Obj = pointGameObject; //Makes node aware of the gameObject.
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
}
