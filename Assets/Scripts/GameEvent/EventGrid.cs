using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventGrid : MonoBehaviour
{
    private InventoryController _controller;
    private PlayerView _player;
    private EventGame _event;

    [SerializeField] private ItemGrid eventItemGrid;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private TMP_Text prompt;
    [SerializeField] private Button btnConfirm;

    private bool _isConfirm;

    private void Start()
    {
        btnConfirm.onClick.AddListener(Complete);
        GameManager.Instance.m_OnEvent.AddListener(InitEvent);
        _controller = GetComponent<InventoryController>();
        _player = GameManager.Instance.GetPlayer();
    }

    public void Complete()
    {
        if (_isConfirm)
        {
            if (_player.GetCurrentNode().Obj.GetComponent<MapNode>().IsMarket)
            {
                GameManager.Instance.OpenMarket();
            }
            else
            {
                GameManager.Instance.m_OffEvent.Invoke();
            }
        }
        else
        {
            _isConfirm = true;
            var items = eventItemGrid.GetItemsInGrid();
            if (CheckRequirements(items))
            {
                eventItemGrid.ClearGrid();
                foreach(var item in _event.eventData.reward)
                {
                    CreateItem(item);
                }
            }
            else
            {
                eventItemGrid.ClearGrid();
                if(_event.eventData.penalty != null)_event.eventData.penalty.RunPenalty(this);
            }
        }
    }

    public void InitEvent()
    {
        
        _isConfirm = false;
        _event = _player.GetCurrentNode().Obj.GetComponent<MapNode>().EventGame;
        eventItemGrid.ClearGrid();
        prompt.text = _event.eventData.prompt;
    }

    public TradeItem CreateItem(ItemData itemData)
    {
        // Other values can still depend on the market.
        var item = _controller.InsertItem(eventItemGrid, itemPrefab, itemData);
        return SetItem(item, itemData);
    }

    private TradeItem SetItem(GameObject item, ItemData data)
    {
        var tradeItem = item.GetComponent<TradeItem>();
        tradeItem.Gold = Random.Range(data.gold.x, data.gold.y + 1);
        tradeItem.Protection = Random.Range(data.protection.x, data.protection.y + 1);
        tradeItem.Tools = Random.Range(data.tools.x, data.tools.y + 1);
        tradeItem.Food = Random.Range(data.food.x, data.food.y + 1);
        return tradeItem;
    }

    private bool CheckRequirements(InventoryItem[] items)
    {
        if (items.Length == 0)
        {
            return false;
        }

        int value = 0;
        int gold = 0;
        int protection = 0;
        int tools = 0;
        int food = 0;

        foreach (var item in items)
        {
            var tradeItem = ((InventoryTradeItem)item).tradeItem;
            value += tradeItem.Value;
            gold += tradeItem.Gold;
            protection += tradeItem.Protection;
            tools += tradeItem.Tools;
            food += tradeItem.Food;
        }

        if (value < _event.eventData.value || gold < _event.eventData.gold || protection < _event.eventData.protection || tools < _event.eventData.tools || food < _event.eventData.food)
        {
            return false;
        }

        return true;
    }
}
