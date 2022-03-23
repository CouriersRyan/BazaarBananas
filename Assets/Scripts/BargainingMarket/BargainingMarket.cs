using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

//Market for bargaining
public class BargainingMarket : MonoBehaviour, ITradeResources
{
    private static readonly float[] WeightCurve = { 0.1f, 1, 1, 2, 3, 3, 2, 1, 1, 0.5f };
    private readonly WeightedRandom _scarcityRandom = new WeightedRandom(WeightCurve);

    private WeightedRandom _itemRandom;
    private float _secondaryChance = 0.5f;
    private float _tertiaryChance = 0.6f;

    private int _marketDefaultCount = 4;

    public int MarketDefaultCount
    {
        get { return _marketDefaultCount; }
    }

    private void Awake()
    {
        gold = _scarcityRandom.GetRandomIndex() - 2;
        gold = gold < 0 ? Random.Range(0, 2) : gold;
        protection = _scarcityRandom.GetRandomIndex();
        tools = _scarcityRandom.GetRandomIndex();
        food = _scarcityRandom.GetRandomIndex();

        float[] temp = { gold, protection, tools, food };
        _itemRandom = new WeightedRandom(temp);
    }

    public TradeItem SetItem(GameObject item, ItemData data)
    {
        var tradeItem = item.GetComponent<TradeItem>();
        if (tradeItem == null)
        {
            tradeItem = item.AddComponent<TradeItem>();
            item.GetComponent<InventoryTradeItem>().tradeItem = tradeItem;
        }

        tradeItem.SetTradeItem(Random.Range(data.gold.x, data.gold.y + 1),
            Random.Range(data.protection.x, data.protection.y + 1), Random.Range(data.tools.x, data.tools.y + 1),
            Random.Range(data.food.x, data.food.y + 1), data.itemName);

        SetItemValue(tradeItem, data);

        return tradeItem;
    }

    //Chooses the primary attribute of the item.
    public TradeResources ChoosePrimary()
    {
        //Choose primary
        var primary = _itemRandom.GetRandomIndex();
        return (TradeResources)primary;
    }

    public virtual void SetItemValue(TradeItem item, ItemData data)
    {
        if (data.value < 0)
        {
            item.Value = 0;
            item.Value += item.Gold * (10 - gold);
            item.Value += item.Protection * (10 - protection);
            item.Value += item.Tools * (10 - tools);
            item.Value += item.Food * (10 - food);
        }
        else
        {
            item.Value = data.value;
        }
    }

    public virtual void OnCloseMarket()
    {
        
    }

    public void RemoveMarketFromCloseEvent()
    {
        GameManager.Instance.m_OffEvent.RemoveListener(OnCloseMarket);
        GameManager.Instance.m_OffEvent.RemoveListener(RemoveMarketFromCloseEvent);
    }
    
    // Determines resource scarcity for trading with the player.
    [SerializeField] private int gold;
    [SerializeField] private int protection;
    [SerializeField] private int tools;
    [SerializeField] private int food;
    
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    } // White

    public int Protection
    {
        get { return protection; }
        set { protection = value; }
    } // Red

    public int Tools
    {
        get { return tools; }
        set { tools = value; }
    } // Blue

    public int Food
    {
        get { return food; }
        set { food = value; }
    } // Green
}