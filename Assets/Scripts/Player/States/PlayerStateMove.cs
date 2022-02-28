using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for when the pawn is in the middle of moving between nodes on the map.
public class PlayerStateMove : PlayerStateBase
{
    public override void StateEnter(PlayerController controller)
    {
        
    }

    public override void StateUpdate(PlayerController controller)
    {
        controller.model.elapsedTime += Time.deltaTime;
        controller.MoveToTargetNode(controller.model.LerpValue);
        if (controller.model.LerpValue >= 1f)
        {
            controller.SetState(controller.model.stateSelect);
        }
        
    }

    public override void StateExit(PlayerController controller)
    {
        controller.model.currentNode = controller.model.targetNode;
    }
}
