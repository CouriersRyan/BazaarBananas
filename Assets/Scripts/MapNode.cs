using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    private EventGame _eventGame;
    private Market _market;

    public Market Market
    {
        get { return _market; }
    }
    public EventGame EventGame
    {
        get { return _eventGame; }
    }

    private bool _isMarket; // Whether the node is for a market or an event.
    public bool IsMarket
    {
        get { return _isMarket; }
    }
    

    private Node node;

    // Determines which nodes are markets and which ones are events.
    void Start()
    {
        _isMarket = node.isExcluded; // Make the node a market if it was excluded in the A Star search.
        if (node.GCost == 0) _isMarket = true; // First node is always a market.
        _market = gameObject.AddComponent<Market>();
        _eventGame = gameObject.AddComponent<EventGame>();
    }
    
    // Set the Node that this MapNode is associated to.
    public void SetNode(Node node)
    {
        this.node = node;
    }
    
    // Returns the Node that this object is associated to.
    public Node GetNode()
    {
        return node;
    }
}
