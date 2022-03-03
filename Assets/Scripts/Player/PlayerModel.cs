using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Model portion of the Player MVC
// Holds resources the player has and values for lerping.
[Serializable] public class PlayerModel : ITradeResources
{

    // For trading.
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
