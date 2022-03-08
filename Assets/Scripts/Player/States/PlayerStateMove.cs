using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for when the pawn is in the middle of moving between nodes on the map.
public class PlayerStateMove : PlayerStateBase
{
    private bool _isDoneFacing;
    
    public override void StateEnter(PlayerController controller, PlayerView view)
    {
        _isDoneFacing = false;
        controller.SetLookAtTarget();
    }

    // Lerp to the new node and move to the event state.
    public override void StateUpdate(PlayerController controller, PlayerView view)
    {
        // Rotate to face the target node.
        controller.model.elapsedTime += Time.deltaTime;
        if (!_isDoneFacing)
        {
            _isDoneFacing = controller.FaceTarget(controller.model.elapsedTime);
            if (_isDoneFacing) controller.model.elapsedTime = 0;
        }
        else
        {
            // Move to target node.
            controller.MoveToTargetNode(controller.model.LerpValue);
        }

        if (controller.model.LerpValue >= 1f)
        {
            view.SetState(view.stateEvent);
        }
        
    }

    public override void StateExit(PlayerController controller, PlayerView view)
    {
        controller.model.currentNode = controller.model.targetNode;
    }
}
