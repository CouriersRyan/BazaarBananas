using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtistPenalty : Penalty
{
    public override void RunPenalty(EventGrid grid)
    {
        var curEvent = GameManager.Instance.GetPlayer().GetCurrentNode().Obj.GetComponent<MapNode>().EventGame;

        var item = grid.CreateItem(curEvent.eventData.reward[0]);
        item.Gold = 5;
    }
}
