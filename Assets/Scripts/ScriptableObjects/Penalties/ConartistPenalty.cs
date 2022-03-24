using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Penalty", menuName = "ScriptableObjects/Penalty", order = 4)]
public class ConartistPenalty : Penalty
{
    public override void RunPenalty(EventGrid grid)
    {
        var itemGrid = GameManager.Instance.GetPlayer().GetInventory();
        var items = itemGrid.GetItemsInGrid();
        itemGrid.CleanItemFromGrid(items[Random.Range(0, items.Length)]);
    }
}
