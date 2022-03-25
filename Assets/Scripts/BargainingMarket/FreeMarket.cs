using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeMarket : BargainingMarket
{

    public override void SetItemValue(TradeItem item, ItemData data)
    {
        item.Value = 0;
    }
}
