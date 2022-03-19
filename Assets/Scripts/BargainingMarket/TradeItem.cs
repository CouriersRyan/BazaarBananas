using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeItem : MonoBehaviour, ITradeResources
{

    public void SetTradeItem(int gold, int protection, int tools, int food)
    {
        Gold = gold;
        Protection = protection;
        Tools = tools;
        Food = food;
    }
    
    public int Value
    {
        get;
        set;
    } // Buy/Sell value in specific market.
    
    public int Gold
    {
        get;
        set;
    } // White
    public int Protection
    {
        get;
        set;
    } // Red
    public int Tools
    {
        get;
        set;

    } // Blue
    public int Food
    {
        get;
        set;

    } // Green
    
}
