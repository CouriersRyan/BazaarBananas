using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Handles interactions between the inventory system and the rest of the game systems.
[RequireComponent(typeof(InventoryController))]
public class MarketGrid : MonoBehaviour
{
    private InventoryController _controller;
    private PlayerView _player;
    private BargainingMarket _market;
    [SerializeField] private ItemGrid marketItemGrid;
    [SerializeField] private ItemGrid extraItemGrid;
    [SerializeField] private GameObject itemPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<InventoryController>();
        _player = GameManager.Instance.GetPlayer();
        GameManager.Instance.m_OnEvent.AddListener(InitMarket);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _controller.PickUpPlaceItem(PickedUpFromMarket, PlacedInMarket);
        }
    }

    private void InitMarket()
    {
        _market = _player.GetCurrentNode().Obj.GetComponent<MapNode>().Market;
        marketItemGrid.ClearGrid();
        extraItemGrid.ClearGrid();
        for (int i = 0; i < _market.MarketDefaultCount; i++)
        {
            CreateItem();
        }
    }

    private void CreateItem()
    {
        var item = _controller.InsertRandomItem(marketItemGrid, itemPrefab);
        _market.CreateItem(item);
    }

    private void PlacedInMarket(InventoryItem placedItem)
    {
        if (_controller.SelectedItemGrid == marketItemGrid)
        {
            if (_controller.SelectedItem != null)
            {
                var selectedItem = _controller.SelectedItem.GetComponent<TradeItem>();
                if (_player.CheckGold(-selectedItem.Value))
                {
                    _player.ChangeGold(-selectedItem.Value);
                }
                else
                {
                    _controller.PickUpPlaceItem();
                    return;
                }
            }
            
            var item = placedItem.GetComponent<TradeItem>();
            _player.ChangeGold(item.Value);
        }
    }
    
    private void PickedUpFromMarket()
    {
        if (_controller.SelectedItemGrid == marketItemGrid)
        {
            if (_controller.SelectedItem != null)
            {
                var selectedItem = _controller.SelectedItem.GetComponent<TradeItem>();
                if (_player.CheckGold(-selectedItem.Value))
                {
                    _player.ChangeGold(-selectedItem.Value);
                }
                else
                {
                    _controller.PickUpPlaceItem();
                }
            }
        }
    }
}
