using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    [SerializeField] private ItemData[] itemDataGold;
    [SerializeField] private ItemData[] itemDataProtection;
    [SerializeField] private ItemData[] itemDataTools;
    [SerializeField] private ItemData[] itemDataFood;
    [SerializeField] private ItemData defaultItem;

    [SerializeField] private ItemPreview preview;
    [SerializeField] private ItemPreview previewOverlap;

    [SerializeField] private TMP_Text textTimer;
    [SerializeField] private Button btnReroll;
    [SerializeField] private float timer = 120f;
    [SerializeField] private float rerollCost = 5;

    private bool _onMarket;


    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<InventoryController>();
        _player = GameManager.Instance.GetPlayer();
        GameManager.Instance.m_OnMarket.AddListener(InitMarket);
        btnReroll.onClick.AddListener(RerollMarket);

        GameManager.Instance.m_OffEvent.AddListener(() =>
        {
            _onMarket = false;
        });
    }


    private void Update()
    {
        timer -= Time.deltaTime;
        textTimer.text = ((int)timer).ToString();
        if (timer <= 0 && _onMarket)
        {
            GameManager.Instance.m_OffEvent.Invoke();
        }
        PreviewItemStats();
        if (Input.GetMouseButtonDown(0))
        {
            _controller.PickUpPlaceItem(PickedUpFromMarket, PlacedInMarket);
        }
    }

    // Sets up a values for the market in the current node.
    private void InitMarket()
    {
        _onMarket = true;
        timer = 120f;
        textTimer.text = ((int)timer).ToString();
        _market = _player.GetCurrentNode().Obj.GetComponent<MapNode>().Market;
        marketItemGrid.ClearGrid();
        extraItemGrid.ClearGrid();
        SetPlayerItemValues();
        for (int i = 0; i < _market.MarketDefaultCount; i++)
        {
            CreateItem();
        }
    }

    // Sets new values for the items in the player's inventory based on the current market.
    private void SetPlayerItemValues()
    {
        var playerItems = _player.GetInventory().GetItemsInGrid();
        if (playerItems.Length == 0)
        {
            return;
        }
        foreach (var item in playerItems)
        {
            _market.SetItemValue(((InventoryTradeItem)(item)).tradeItem, item.itemData);
        }
    }

    private void CreateItem()
    {
        // Other values can still depend on the market.
        var itemData = GetRandomItemData(_market.ChoosePrimary());
        var item = _controller.InsertItem(marketItemGrid, itemPrefab, itemData);
        if (item != null)
        {
            _market.SetItem(item, itemData);
        }
    }

    private void PreviewItemStats()
    {
        if (_controller.ItemToHighlight != null)
        {
            preview.gameObject.SetActive(true);
            preview.ShowPreview(((InventoryTradeItem)_controller.ItemToHighlight).tradeItem, Input.mousePosition.x,
                Input.mousePosition.y);

            if (_controller.ItemOverlapHighlight != null)
            {
                previewOverlap.gameObject.SetActive(true);
                previewOverlap.SetPivot(preview.Pivot.x, preview.Pivot.y);
                if (previewOverlap.ShowPreview(((InventoryTradeItem)_controller.ItemOverlapHighlight).tradeItem,
                        Input.mousePosition.x + preview.Size.x * preview.CanvasScaleFactor,
                        Input.mousePosition.y))
                {
                    preview.ShowPreview(((InventoryTradeItem)_controller.ItemToHighlight).tradeItem, previewOverlap.Pos.x - preview.Size.x * preview.CanvasScaleFactor,
                        previewOverlap.Pos.y);
                }
            }
            else
            {
                previewOverlap.gameObject.SetActive(false);
            }
        }
        else
        {
            preview.gameObject.SetActive(false);
            previewOverlap.gameObject.SetActive(false);
        }
    }


    private ItemData GetRandomItemData(TradeResources primary)
    {
        ItemData[] items = null;

        switch (primary)
        {
            case TradeResources.Gold:
                items = itemDataGold;
                break;
            
            case TradeResources.Protection:
                items = itemDataProtection;
                break;
            
            case TradeResources.Tools:
                items = itemDataTools;
                break;
            
            case TradeResources.Food:
                items = itemDataFood;
                break;
        }

        if (items != null)
        {
            return items[Random.Range(0, items.Length)];
        }
        
        return defaultItem;
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

    private void RerollMarket()
    {
        if (timer > rerollCost)
        {
            timer -= rerollCost;
            marketItemGrid.ClearGrid();
            for (int i = 0; i < _market.MarketDefaultCount; i++)
            {
                CreateItem();
            }
        }
    }
    
}
