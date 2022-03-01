using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The base state for the Player FSM of which all other states are children of this class.
public class PlayerStateBase
{
    public virtual void StateEnter(PlayerController controller, PlayerView view)
    {
        
    }

    public virtual void StateUpdate(PlayerController controller, PlayerView view)
    {
        
    }

    public virtual void StateExit(PlayerController controller, PlayerView view)
    {
        
    }
}
