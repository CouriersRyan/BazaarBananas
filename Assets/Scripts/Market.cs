using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TradeResources
{
    Gold,
    Protection,
    Tools,
    Food
}

public class Market : MonoBehaviour, ITradeResources
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeMarket()
    {
        //TODO Implement
    }
    
    // Determines resources for trading with the player.
    [SerializeField] private int gold;
    [SerializeField] private int protection;
    [SerializeField] private int tools;
    [SerializeField] private int food;
    
    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    } // White
    public int Protection { 
        get
        {
            return protection;
        }
        set
        {
            protection = value;
        } 
    } // Red
    public int Tools {
        get
        {
            return tools;
        }
        set
        {
            tools = value;
        }
        
    } // Blue
    public int Food {
        get
        {
            return food;
        }
        set
        {
            food = value;
        }
        
    } // Green
}
