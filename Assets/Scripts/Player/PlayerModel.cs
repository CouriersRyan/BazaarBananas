using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Model portion of the Player MVC
// Holds resources the player has and values for lerping.
[Serializable] public class PlayerModel : ITradeResources
{

    // For trading.
    public int Gold { get; set; } // Gold
    public int Protection { get; set; } // Red
    public int Tools { get; set; } // Blue
    public int Food { get; set; } // Green
    
    // For map navigation
    [NonSerialized] public Node currentNode;
    [NonSerialized] public Node targetNode;

    [SerializeField] public Transform pawn; // The player's visual character.
    
    // For lerping the pawn.
    [NonSerialized] public Vector2 lerpTo;
    [NonSerialized] public Vector2 lerpFrom;
    [NonSerialized] public Quaternion rotateFrom;
    [NonSerialized] public Quaternion rotateAt;
    [NonSerialized] public float elapsedTime;
    [SerializeField] private float maxTime;
    public float LerpValue
    {
        get
        {
            return elapsedTime / maxTime;
        }
    }
    

}
