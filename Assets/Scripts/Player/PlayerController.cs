using System.Collections;
using System.Collections.Generic;
using DelaunatorSharp.Unity.Extensions;
using UnityEngine;
using UnityEngine.VFX;

// The controller portion of the Player MVC
// Mainly handles the player navigating the on screen map and receiving choices made from the event and market menus.
public class PlayerController
{
    public PlayerModel model; // Reference to the Model in the MVC.
    
    // Initializes the class and sets the state and pawn to starting positions.
    public PlayerController(PlayerModel model)
    {
        this.model = model;
        this.model.currentNode = GameManager.Instance.GetMap().StartNode;
        this.model.pawn.position = this.model.currentNode.ToVector2();
    }

    // Checks if the selected node is valid for moving. If it is, move to it.
    public bool CheckValidNode(Node target)
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
        }

        return isValid;
    }
    
    // Set the selected node for the pawn to move to.
    public void SetTargetNode(Node target)
    {
        model.lerpTo = target.ToVector2();
        model.lerpFrom = model.currentNode.ToVector2();
        model.targetNode = target;
        model.elapsedTime = 0;
    }

    // Lerp to the next node from the current node.
    public Vector2 MoveToTargetNode(float time)
    {
        return model.pawn.position = Vector2.Lerp(model.lerpFrom, model.lerpTo, time);
    }
    
    // Set the selected node for the pawn to rotate to face.
    public void SetLookAtTarget()
    {
        var faceVector = ((Vector2)model.pawn.position - model.lerpTo).normalized;
        model.rotateAt = Quaternion.LookRotation(faceVector, Vector3.back);
        model.rotateFrom = model.pawn.rotation;
        model.elapsedTime = 0;
    }
    
    // Lerp to rotate to face the next node.
    public void FaceTarget(float time)
    {
        model.pawn.rotation = Quaternion.Slerp(model.rotateFrom, model.rotateAt, time);
    }
    
}
