using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Pathfinding algorithm to find the shortest path from two nodes.
// Class was created by me using this youtube video tutorial as reference: https://www.youtube.com/watch?v=alU04hvz6L4
// A* Pathfinding in Unity by Code Monkey
// The original video details the algorithm for an even 2D grid, but I adopted it for my graph of nodes that are not
// evenly distanced.
public class AStar
{
    private List<Node> _openNodes;
    private List<Node> _closedNodes;
    private List<Node> _allNodes;

    private List<List<Node>> _paths = new List<List<Node>>();

    // Runs find path multiple times, excluding a random node each time to create a variety of paths.
    public List<List<Node>> FindPaths(Node start, Node end, List<Node> allNodesList, int reps)
    {
        _paths.Clear();
        for (int i = 0; i < reps; i++)
        {
            var currPath = FindPath(start, end, allNodesList);
            _paths.Add(currPath);
            ExcludeRandomNode(currPath);
        }

        return _paths;
    }

    // Returns a list of nodes that is the path found using A*
    public List<Node> FindPath(Node start, Node end, List<Node> allNodesList)
    {
        _allNodes = allNodesList; // List of all nodes for pathfinding.
        _openNodes = new List<Node> {start}; // List of nodes to check
        _closedNodes = new List<Node>(); // List of nodes that have already been checked.
        
        // Resets any potential leftovers from previous runs of FindPath.
        foreach (var node in _allNodes)
        {
            node.GCost = int.MaxValue;
            node.PrevNode = null;
        }
        
        // Sets costs for the start node.
        start.GCost = 0;
        start.HCost = CalculateDistance(start, end);
        
        // Runs while there are still nodes available to check.
        while (_openNodes.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(); // Checks nodes in order of cost (lowest -> highest)
            
            // Return once a path to the end has been found.
            if (currentNode == end)
            {
                return Path(end);
            }
            
            _openNodes.Remove(currentNode);
            _closedNodes.Add(currentNode);
            
            // Checks the nodes linked to the current node.
            foreach (var node in currentNode.Links)
            {
                // Continue to next linked node if current node has already been checked or is excluded from the path.
                if(_closedNodes.Contains(node)) continue;
                if (node.isExcluded)
                {
                    _closedNodes.Add(node);
                    continue;
                }
                
                // Calculate a G cost for the node using the cost of the previous node and distance between the two nodes.
                float gCost = currentNode.GCost + CalculateDistance(currentNode, node);
                
                // If the new calculated G cost is less than the previous G cost assigned (i.e. the path is better).
                // then have that node be part of the lower cost path.
                if (gCost < node.GCost)
                {
                    node.PrevNode = currentNode;
                    node.GCost = gCost;
                    node.HCost = CalculateDistance(node, end);

                    if (!_openNodes.Contains(node))
                    {
                        _openNodes.Add(node);
                    }
                }
            }
        }
        
        return null; // If no path was found.
    }

    // Returns distance between two nodes.
    private float CalculateDistance(Node start, Node end)
    {
        return Vector2.Distance(new Vector2((float)start.X, (float)start.Y), new Vector2((float)end.X, (float)end.Y));
    }

    // Find node with the lowest FCost in terms of A*
    private Node GetLowestFCostNode()
    {
        var lowestCostNode = _openNodes[0];
        foreach (var node in _openNodes)
        {
            if (node.FCost < lowestCostNode.FCost)
            {
                lowestCostNode = node;
            }
        }

        return lowestCostNode;
    }

    // Returns a path to the given node.
    private List<Node> Path(Node end)
    {
        List<Node> path = new List<Node>();
        path.Add(end);
        var node = end;
        while (node.PrevNode != null)
        {
            path.Add(node.PrevNode);
            node = node.PrevNode;
        }
        path.Reverse();
        return path;
    }

    // Excludes a random node from the pathfinding.
    // Cannot exclude beginning and end nodes because they are always on the path.
    private void ExcludeRandomNode(List<Node> path)
    {
        path[Random.Range(1, path.Count - 1)].isExcluded = true;
    }
}
