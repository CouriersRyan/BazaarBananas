using System.Collections;
using System.Collections.Generic;
using DelaunatorSharp.Unity.Extensions;
using UnityEngine;

public class PlayerController
{
    public PlayerModel model;
    public PlayerController(PlayerModel model)
    {
        this.model = model;
        this.model.currentNode = GameManager.Instance.GetMap().StartNode;
        this.model.pawn.position = this.model.currentNode.ToVector2();
        SetState(this.model.stateSelect);
    }

    public void Update()
    {
        model.State.StateUpdate(this);
    }

    public void CheckValidNode(Node target)
    {
        bool isValid = false;
        foreach (var node in model.currentNode.Links)
        {
            if (target == node)
            {
                isValid = true;
            }
        }
        if (isValid)
        {
            SetTargetNode(target);
            SetState(model.stateMove);
        }
    }
    
    public void SetTargetNode(Node target)
    {
        model.lerpTo = target.ToVector2();
        model.lerpFrom = model.currentNode.ToVector2();
        model.targetNode = target;
        model.elapsedTime = 0;
    }

    public Vector2 MoveToTargetNode(float time)
    {
        return model.pawn.position = Vector2.Lerp(model.lerpFrom, model.lerpTo, time);
    }

    public void SetState(PlayerStateBase newState)
    {
        if (model.State != null)
        {
            model.State.StateExit(this);
        }

        model.State = newState;

        if (model.State != null)
        {
            model.State.StateEnter(this);
        }
    }
}
