using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    private ItemGrid _selectedItemGrid;
    
    public ItemGrid SelectedItemGrid
    {
        get
        {
            return _selectedItemGrid;
        }

        set
        {
            _selectedItemGrid = value;
            _highlight.AssignParent(value);
        }
    }

    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvas;

    private InventoryHighlight _highlight;


    private InventoryItem _itemToHighlight;
    private Vector2Int _oldPosition;
    private InventoryItem _overlapItem;
    private RectTransform _rectTransformOfSelectedItem;

    private InventoryItem _selectedItem;

    private void Awake()
    {
        _highlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (SelectedItemGrid == null)
        {
            _highlight.Show(false);
            return;
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            var tileGridPos = GetTileGridPos();

            if (_selectedItem == null)
            {
                PickUpItem(tileGridPos);
            }
            else
            {
                PlaceItem(tileGridPos);
            }
        }
    }

    private Vector2Int GetTileGridPos()
    {
        Vector2 position = Input.mousePosition;

        if (_selectedItem != null)
        {
            position.x -= (_selectedItem.itemData.width - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (_selectedItem.itemData.height - 1) * ItemGrid.TileSizeHeight / 2;
        }

        return SelectedItemGrid.GetTileGridPosition(position);
    }

    
    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPos();

        if (_oldPosition == positionOnGrid)
        {
            return;
        }
        
        _oldPosition = positionOnGrid;

        if (_selectedItem == null)
        {
            _itemToHighlight = SelectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (_itemToHighlight != null)
            {
                _highlight.Show(true);
                _highlight.SetSize(_itemToHighlight);
                _highlight.SetPosition(SelectedItemGrid, _itemToHighlight);
            }
            else
            {
                _highlight.Show(false);
            }
        }
        else
        {
            _highlight.Show(SelectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y,
                _selectedItem.itemData.width, _selectedItem.itemData.height));
            _highlight.SetSize(_selectedItem);
            _highlight.SetPosition(SelectedItemGrid, _selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    public void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        var rectTransform = inventoryItem.RectTransform;
        rectTransform.SetParent(canvas);

        _selectedItem = inventoryItem;
        _rectTransformOfSelectedItem = rectTransform;

        int selectedItemID = Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
    }

    private void PlaceItem(Vector2Int tileGridPos)
    {
        bool success = SelectedItemGrid.PlaceItem(_selectedItem, tileGridPos.x, tileGridPos.y, ref _overlapItem);
        if (success)
        {
            _selectedItem = null;
            if (_overlapItem != null)
            {
                _selectedItem = _overlapItem;
                _overlapItem = null;
                _rectTransformOfSelectedItem = _selectedItem.RectTransform;
            }
        }
    }

    private void PickUpItem(Vector2Int tileGridPos)
    {
        _selectedItem = SelectedItemGrid.PickUpItem(tileGridPos.x, tileGridPos.y);
        if (_selectedItem != null)
        {
            _rectTransformOfSelectedItem = _selectedItem.RectTransform;
        }
    }

    private void ItemIconDrag()
    {
        if (_selectedItem != null)
        {
            _rectTransformOfSelectedItem.position = Input.mousePosition;
        }
    }
}