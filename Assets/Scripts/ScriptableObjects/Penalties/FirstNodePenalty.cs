using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstNodePenalty : Penalty
{
    public override void RunPenalty(EventGrid grid)
    {
        var playerView = GameManager.Instance.GetPlayer();
        if (playerView.GetCurrentNode().GCost == 0)
        {
            playerView.ChangeGold(-playerView.GetGold(TradeResources.Gold));
        }
        else
        {
            playerView.ChangeGold(-100);
            if (!playerView.CheckGold(-1))
            {
                GameManager.Instance.m_GameOver.Invoke();
            }
        }
    }
}
