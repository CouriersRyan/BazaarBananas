using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Market for bargaining
public class BargainingMarket : MonoBehaviour, ITradeResources
{
    //TODO: have items on screen to represent the list of market items. Should be able to click on them to buy and sell.
    //TODO: hover over to get info.
    //TODO: implement grid based inventory.
    private static readonly float[] WeightCurve = { 0.01f, 0.1f, 1, 1, 2, 3, 3, 2, 1, 1, 0.5f, 0.02f };
    private readonly WeightedRandom _scarcityRandom = new WeightedRandom(WeightCurve);

    private WeightedRandom _itemRandom;
    private float _secondaryChance = 0.5f;
    private float _tertiaryChance = 0.6f;

    private int _marketSize = 3;

    public List<TradeItem> ItemInMarket = new List<TradeItem>();

    private void Awake()
    {
        gold = _scarcityRandom.GetRandomIndex() - 2;
        gold = gold < 0 ? Random.Range(0, 2) : gold;
        protection = _scarcityRandom.GetRandomIndex();
        tools = _scarcityRandom.GetRandomIndex();
        food = _scarcityRandom.GetRandomIndex();

        float[] temp = { gold, protection, tools, food };
        _itemRandom = new WeightedRandom(temp);
        
        int i = 0;
        while (i < _marketSize)
        {
            ItemInMarket.Add(CreateItem());
            i++;
        }

    }

    public TradeItem CreateItem()
    {
        int[] resources = new int[4];
        
        //Choose primary
        var primary = _itemRandom.GetRandomIndex();
        
        //Check if it will have secondary, or tertiary, etc.
        var isSecondary = Random.Range(0f, 1f) < _secondaryChance;
        var isTertiary = false;
        if(isSecondary) isTertiary = Random.Range(0f, 1f) < _tertiaryChance;

        //Calculate a size.
        int size = Random.Range(0, 12) + 1;

        resources[primary] = Random.Range(0, size) + 1;
        size -= resources[primary];

        if (isSecondary)
        {
            if (isTertiary)
            {
                float[] temp = { 1, 1, 1, 1 };
                var choose = new WeightedRandom(temp);
                choose.SetWeight(primary, 0);
                do
                {
                    int i = choose.GetRandomIndex();
                    choose.SetWeight(i, 0);
                    resources[i] = Random.Range(0, size) + 1;
                    size -= resources[i];
                } while (choose.GetTotalWeight() > 0 && size > 0);
            }
            else
            {
                var secondary = 0;
                do
                {
                    secondary = _itemRandom.GetRandomIndex();
                } while (secondary != primary);

                resources[secondary] = size;
            }
        }

        var item = new TradeItem(resources[0], resources[1], resources[2], resources[3]);
        SetItemValue(item);

        return item;
    }

    public void SetItemValue(TradeItem item)
    {
        item.Value = 0;
        item.Value += item.Gold * (10 - gold);
        item.Value += item.Protection * (10 - protection);
        item.Value += item.Tools * (10 - tools);
        item.Value += item.Food * (10 - food);
    }

    // Determines resource scarcity for trading with the player.
    [SerializeField] private int gold;
    [SerializeField] private int protection;
    [SerializeField] private int tools;
    [SerializeField] private int food;
    
    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold = value;
        }
    } // White
    public int Protection { 
        get
        {
            return protection;
        }
        set
        {
            protection = value;
        } 
    } // Red
    public int Tools {
        get
        {
            return tools;
        }
        set
        {
            tools = value;
        }
        
    } // Blue
    public int Food {
        get
        {
            return food;
        }
        set
        {
            food = value;
        }
        
    } // Green
}
