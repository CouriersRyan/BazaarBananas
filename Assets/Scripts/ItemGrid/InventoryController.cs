using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    [HideInInspector] public ItemGrid selectedItemGrid;

    private InventoryItem _selectedItem;
    private RectTransform _rectTransformOfSelectedItem;

    [SerializeField] private List<ItemData> items;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform canvas;
    private void Update()
    {
        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }
        
        if (selectedItemGrid == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var tileGridPos = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

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
        bool success = selectedItemGrid.PlaceItem(_selectedItem, tileGridPos.x, tileGridPos.y);
        if(success) _selectedItem = null;
    }

    private void PickUpItem(Vector2Int tileGridPos)
    {
        _selectedItem = selectedItemGrid.PickUpItem(tileGridPos.x, tileGridPos.y);
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
