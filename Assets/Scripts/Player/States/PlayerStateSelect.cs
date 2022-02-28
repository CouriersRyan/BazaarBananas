using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player state for when the player needs to select the next node to move to.
public class PlayerStateSelect : PlayerStateBase
{
    public override void StateEnter(PlayerController controller)
    {
        
    }

    public override void StateUpdate(PlayerController controller)
    {
        if (Input.GetMouseButtonUp(0))
        {
            // Copied from Match3-FSM assignment
            // Send an imaginary ray into the screen at the position of the mouse click and return the object that is located there.
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                // If the player clicks a node linked to the current node, move to that node.
                if (hit.collider.CompareTag("Node"))
                {
                    MapNode mapNode = hit.collider.gameObject.GetComponent<MapNode>();
                    controller.CheckValidNode(mapNode.GetNode());
                }
            }
        }
    }

    public override void StateExit(PlayerController controller)
    {
        
    }
}
