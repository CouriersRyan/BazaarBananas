using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class used to determine the rate at which the player can sell and buy in a given market.
public class Market : MonoBehaviour, ITradeResources
{
    // Determines resources for trading with the player.
    [SerializeField] private int gold;
    [SerializeField] private int protection;
    [SerializeField] private int tools;
    [SerializeField] private int food;

    private int[,] resourceConversions = new int[4, 4];
    
    // Start is called before the first frame update
    void Awake()
    {
        RandomizeMarket();
    }

    
    // Randomizes the rate at which resources can be converted to gold at this market.
    public void RandomizeMarket()
    {
        resourceConversions[(int)TradeResources.Gold, (int)TradeResources.Protection] = Random.Range(5, 11);
        resourceConversions[(int)TradeResources.Gold, (int)TradeResources.Food] = Random.Range(5, 11);
        resourceConversions[(int)TradeResources.Gold, (int)TradeResources.Tools] = Random.Range(5, 11);
    }

    // Get the conversions between two resources.
    // Currently only used between gold and another resource. Theoretically, if I kept working on this, it would
    // Allow for conversions between two of any resource.
    public int GetConversion(TradeResources resource0, TradeResources resource1)
    {
        return resourceConversions[(int)resource0, (int)resource1]; 
    }

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
