using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
