using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGrid : MonoBehaviour
{
    public const float TileSizeWidthUnscaled = 100f;
    public const float TileSizeHeightUnscaled = 100f;
    public static float TileSizeWidth {
        get
        {
            if (_canvas == null)
            {
                _canvas = FindObjectOfType<Canvas>();
            }
            Debug.Log(_canvas.scaleFactor);
            return TileSizeWidthUnscaled * _canvas.scaleFactor;
        } 
    }
    public static float TileSizeHeight {
        get
        {
            if (_canvas == null)
            {
                _canvas = FindObjectOfType<Canvas>();
            }
            return TileSizeWidthUnscaled * _canvas.scaleFactor;
        } 
    }

    private static Canvas _canvas;

    private InventoryItem[,] _inventoryItemSlots;

    private RectTransform _rectTransform;

    [SerializeField] private int gridSizeWidth = 8;
    [SerializeField] private int gridSizeHeight = 5;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        InitGrid(gridSizeWidth, gridSizeHeight);
    }

    private void InitGrid(int width, int height)
    {
        _inventoryItemSlots = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * TileSizeWidthUnscaled, height * TileSizeHeightUnscaled);
        _rectTransform.sizeDelta = size;
    }


    private Vector2 _positionOnGrid = new Vector2();
    private Vector2Int _tileGridPosition = new Vector2Int();

    // Returns the tile the mouse grid is on.
    public Vector2Int GetTileGridPosition(Vector2 mousePos)
    {
        _positionOnGrid.x = mousePos.x - _rectTransform.position.x;
        _positionOnGrid.y = _rectTransform.position.y - mousePos.y;
        
        _tileGridPosition.x = (int)(_positionOnGrid.x / TileSizeWidth);
        _tileGridPosition.y = (int)(_positionOnGrid.y / TileSizeHeight);
        
        //Debug.Log(_tileGridPosition);


        return _tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (!BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height))
        {
            return false;
        }

        if (!OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height, inventoryItem, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanItemFromGrid(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.RectTransform;
        rectTransform.SetParent(_rectTransform);

        for (int x = 0; x < inventoryItem.Width; x++)
        {
            for (int y = 0; y < inventoryItem.Height; y++)
            {
                if (inventoryItem.Size(x, y))
                {
                    _inventoryItemSlots[posX + x, posY + y] = inventoryItem;
                }
            }
        }

        inventoryItem.onGridPosX = posX;
        inventoryItem.onGridPosY = posY;

        var position = CalcPosition(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalcPosition(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * TileSizeWidthUnscaled + TileSizeWidthUnscaled * inventoryItem.Width / 2;
        position.y = -(posY * TileSizeHeightUnscaled + TileSizeHeightUnscaled * inventoryItem.Height / 2);
        return position;
    }

    public bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_inventoryItemSlots[posX + x, posY + y] != null)
                {
                    if (overlapItem == null){
                        overlapItem = _inventoryItemSlots[posX + x, posY + y];
                        
                    }
                    else
                    {
                        if (overlapItem != _inventoryItemSlots[posX + x, posY + y])
                        {
                            return false;
                        }
                    }
                }
            }
        }
        
        return true;
    }

    public bool OverlapCheck(int posX, int posY, int width, int height, InventoryItem selectedItem, ref InventoryItem overlapItem)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (selectedItem.Size(x, y))
                {
                    if (_inventoryItemSlots[posX + x, posY + y] != null)
                    {
                        if (overlapItem == null){
                            overlapItem = _inventoryItemSlots[posX + x, posY + y];
                        
                        }
                        else
                        {
                            if (overlapItem != _inventoryItemSlots[posX + x, posY + y])
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        
        return true;
    }
    
    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (_inventoryItemSlots[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        
        return true;
    }
    
    private bool CheckAvailableSpace(int posX, int posY, InventoryItem item)
    {
        for (int x = 0; x < item.Width; x++)
        {
            for (int y = 0; y < item.Height; y++)
            {
                if (_inventoryItemSlots[posX + x, posY + y] != null && item.Size(x, y))
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        var toReturn = _inventoryItemSlots[x, y];

        if (toReturn == null) return null;

        CleanItemFromGrid(toReturn);

        return toReturn;
    }

    private void CleanItemFromGrid(InventoryItem cleanItem)
    {
        for (int x = 0; x < cleanItem.Width; x++)
        {
            for (int y = 0; y < cleanItem.Height; y++)
            {
                if (cleanItem.Size(x, y))
                {
                    _inventoryItemSlots[cleanItem.onGridPosX + x, cleanItem.onGridPosY + y] = null;
                }
            }
        }
    }

    public void ClearGrid()
    {
        var items = gameObject.GetComponentsInChildren<InventoryItem>();
        foreach (InventoryItem item in items)
        {
            Destroy(item.gameObject); //TODO Object Pooling.
        }
        _inventoryItemSlots = new InventoryItem[gridSizeWidth, gridSizeHeight];
    }

    public bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSizeWidth || posY >= gridSizeHeight)
        {
            return false;
        }

        return true;
    }

    public bool BoundaryCheck(int posX, int posY, int width, int height)
    {
        if (!PositionCheck(posX, posY))
        {
            return false;
        }

        posX += width - 1;
        posY += height - 1;
        
        if (!PositionCheck(posX, posY))
        {
            return false;
        }
        
        return true;
    }

    public InventoryItem GetItem(int x, int y)
    {
        return _inventoryItemSlots[x, y];
    }

    public Vector2Int? FindSpaceForItem(InventoryItem itemToInsert)
    {
        int height = gridSizeHeight - itemToInsert.Height + 1;
        int width = gridSizeWidth - itemToInsert.Width + 1;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (CheckAvailableSpace(x, y, itemToInsert))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }

    public InventoryItem[] GetItemsInGrid()
    {
        List<InventoryItem> items = new List<InventoryItem>();
        for (int x = 0; x < gridSizeWidth; x++)
        {
            for (int y = 0; y < gridSizeHeight; y++)
            {
                if (!items.Contains(GetItem(x, y)) && GetItem(x, y) != null)
                {
                    items.Add(GetItem(x, y));
                }
            }
        }

        return items.ToArray();
    }
}
