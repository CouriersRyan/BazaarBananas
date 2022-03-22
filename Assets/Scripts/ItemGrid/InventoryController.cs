using System;
using System.Collections.Generic;
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
    private InventoryItem _itemOverlapHighlight;

    public InventoryItem ItemToHighlight
    {
        get { return _itemToHighlight; }
    }
    public InventoryItem ItemOverlapHighlight
    {
        get { return _itemOverlapHighlight; }
    }
    
    private Vector2Int _oldPosition;
    private InventoryItem _overlapItem;
    private RectTransform _rectTransformOfSelectedItem;

    private InventoryItem _selectedItem;

    public InventoryItem SelectedItem
    {
        get { return _selectedItem; }
    }
    
    private InventoryItem _createdItem;
    private RectTransform _createdItemRectTransform;

    private void Awake()
    {
        _highlight = GetComponent<InventoryHighlight>();
    }

    private void Update()
    {
        ItemIconDrag();
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        HandleHighlight();
    }

    private bool IsGridSelected()
    {
        if (SelectedItemGrid == null)
        {
            _highlight.Show(false);
            _itemToHighlight = null;
            _itemOverlapHighlight = null;
            return true;
        }

        return false;
    }

    public void PickUpPlaceItem()
    {
        if (IsGridSelected()) return;
        
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
    
    public void PickUpPlaceItem(Action onPickUp, Action<InventoryItem> onPlace)
    {
        if (IsGridSelected()) return;
        
        var tileGridPos = GetTileGridPos();

        if (_selectedItem == null)
        {
            if (PickUpItem(tileGridPos))
            {
                onPickUp();
            }
        }
        else
        {
            var placedItem = _selectedItem;
            if (PlaceItem(tileGridPos))
            {
                onPlace(placedItem);
            }
        }
    }

    private void RotateItem()
    {
        if (_selectedItem == null)
        {
            return;
        }

        _selectedItem.Rotate();
    }

    private Vector2Int GetTileGridPos()
    {
        Vector2 position = Input.mousePosition;

        if (_selectedItem != null)
        {
            position.x -= (_selectedItem.Width - 1) * ItemGrid.TileSizeWidth / 2;
            position.y += (_selectedItem.Height - 1) * ItemGrid.TileSizeHeight / 2;
        }

        return SelectedItemGrid.GetTileGridPosition(position);
    }

    
    private void HandleHighlight()
    {
        if (IsGridSelected()) return;
        
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
            bool inBounds = SelectedItemGrid.BoundaryCheck(positionOnGrid.x, positionOnGrid.y,
                _selectedItem.Width, _selectedItem.Height);
            _highlight.Show(inBounds);
            if (inBounds)
            {
                _itemOverlapHighlight = null;
                if (!SelectedItemGrid.OverlapCheck(positionOnGrid.x, positionOnGrid.y,
                        _selectedItem.Width, _selectedItem.Height, _selectedItem, ref _itemOverlapHighlight))
                {
                    _itemOverlapHighlight = null;
                }
                else
                {
                    if(_itemOverlapHighlight == null) {_itemToHighlight = null;}
                    else {_itemToHighlight = _selectedItem;}
                }
            }
            _highlight.SetSize(_selectedItem);
            _highlight.SetPosition(SelectedItemGrid, _selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }
    
    private void InsertRandomItem(ItemGrid grid)
    {
        InsertRandomItem(grid, itemPrefab);
    }

    public GameObject InsertRandomItem(ItemGrid grid, GameObject prefab)
    {
        if (grid == null)
        {
            return null;
        }
        
        CreateRandomItem(prefab);
        InventoryItem itemToInsert = _createdItem;
        _createdItem = null;
        InsertItem(itemToInsert, grid);
        return itemToInsert.gameObject;
    }
    
    public GameObject InsertItem(ItemGrid grid, GameObject prefab, ItemData itemData)
    {
        if (grid == null)
        {
            return null;
        }
        
        CreateItem(prefab, itemData);
        InventoryItem itemToInsert = _createdItem;
        _createdItem = null;
        InsertItem(itemToInsert, grid);
        return itemToInsert.gameObject;
    }

    private void CreateRandomItem(GameObject prefab)
    {
        int selectedItemID = Random.Range(0, items.Count);
        CreateItem(prefab, items[selectedItemID]);
    }
    
    private void CreateItem(GameObject prefab, ItemData itemData)
    {
        InventoryItem inventoryItem = Instantiate(prefab).GetComponent<InventoryItem>();
        var rectTransform = inventoryItem.RectTransform;
        rectTransform.SetParent(canvas);

        _createdItem = inventoryItem;
        _createdItemRectTransform = rectTransform;
        
        inventoryItem.Set(itemData);
    }
    
    private void InsertItem(InventoryItem itemToInsert, ItemGrid itemGrid)
    {

        Vector2Int? posOnGrid = itemGrid.FindSpaceForItem(itemToInsert);

        if (posOnGrid == null)
        {
            return; 
        }

        itemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }
    
    private bool PlaceItem(Vector2Int tileGridPos)
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
                _rectTransformOfSelectedItem.SetParent(_rectTransformOfSelectedItem.parent.parent);
                _rectTransformOfSelectedItem.SetAsLastSibling();
            }

            return true;
        }

        return false;
    }

    private bool PickUpItem(Vector2Int tileGridPos)
    {
        _selectedItem = SelectedItemGrid.PickUpItem(tileGridPos.x, tileGridPos.y);
        if (_selectedItem != null)
        {
            _rectTransformOfSelectedItem = _selectedItem.RectTransform;
            _rectTransformOfSelectedItem.SetParent(_rectTransformOfSelectedItem.parent.parent);
            _rectTransformOfSelectedItem.SetAsLastSibling();
            return true;
        }

        return false;
    }

    private void ItemIconDrag()
    {
        if (_selectedItem != null)
        {
            _rectTransformOfSelectedItem.position = Input.mousePosition;
        }
    }
}