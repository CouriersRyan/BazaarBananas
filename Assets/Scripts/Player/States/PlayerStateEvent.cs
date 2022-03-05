using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for when the player needs to interact with pop-up menus.
public class PlayerStateEvent : PlayerStateBase
{
    // Since this state is just for when the player is interacting with menus, most of the code that runs during this
    // in the respective menu that the player interacts with. This class just tells when a menu is opened and closed.
    public override void StateEnter(PlayerController controller, PlayerView view)
    {
        GameManager.Instance.m_OnEvent.Invoke();
    }

    public override void StateUpdate(PlayerController controller, PlayerView view)
    {
        
    }

    public override void StateExit(PlayerController controller, PlayerView view)
    {
        
    }
}
