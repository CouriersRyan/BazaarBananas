using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerModel : ITradeResources
{
    // For trading.
    public int Gold { get; set; } // Gold
    public int Protection { get; set; } // Red
    public int Tools { get; set; } // Blue
    public int Food { get; set; } // Green
    
    // For map navigation
    private Map _map;
    public Map Map
    {
        get { return _map; }
        set {
            if (_map == null) // Only allow
            {
                _map = value;
            }
        }
    }

    [SerializeField] private Transform pawn;

    public PlayerModel()
    {
        
    }
}
