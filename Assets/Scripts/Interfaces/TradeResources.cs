using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface that holds the types of resources in the game.
public interface ITradeResources
{
    int Gold { get; set; } // Gold
    int Protection { get; set; } // Red
    int Tools { get; set; } // Blue
    int Food { get; set; } // Green
}

// Enum for the list of each type of resource.
public enum TradeResources
{
    Gold = 0,
    Protection = 1,
    Tools = 2,
    Food = 3
}
