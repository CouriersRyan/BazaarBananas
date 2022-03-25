using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

// Class for the objects instantiated to physically represent Nodes on the Map in the Scene.
public class MapNode : MonoBehaviour
{
    // The two possible things the player can open on a nodd.
    private EventGame _eventGame;
    private BargainingMarket _market;

    public BargainingMarket Market
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
        if(node.isExcluded)
        {
            _isMarket = true; // A node has a chance to be a market if it was excluded in the A Star search.
        }
        else
        {
            _isMarket = (Random.Range(0f, 1f) > 0.5f);
        }
        if (node.GCost == 0) _isMarket = true; // First node is always a market.
        _eventGame = gameObject.AddComponent<EventGame>();
        _eventGame.RandomizeEvent(_isMarket);
        if (_isMarket)
        {
            if (node.GCost == 0)
            {
                _market = gameObject.AddComponent<FreeMarket>();
            }
            else
            {
                _market = gameObject.AddComponent<BargainingMarket>();
            }
            
            gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
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
