using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for when the pawn is in the middle of moving between nodes on the map.
public class PlayerStateMove : PlayerStateBase
{
    public override void StateEnter(PlayerController controller, PlayerView view)
    {
        
    }

    public override void StateUpdate(PlayerController controller, PlayerView view)
    {
        controller.model.elapsedTime += Time.deltaTime;
        controller.MoveToTargetNode(controller.model.LerpValue);
        if (controller.model.LerpValue >= 1f)
        {
            view.SetState(view.stateSelect);
        }
        
    }

    public override void StateExit(PlayerController controller, PlayerView view)
    {
        controller.model.currentNode = controller.model.targetNode;
    }
}
