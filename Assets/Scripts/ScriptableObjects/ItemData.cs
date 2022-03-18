using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject, ITradeResources
{
    public int width = 1;
    public int height = 1;

    public Sprite itemIcon;
    
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
