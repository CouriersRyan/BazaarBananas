using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float TileSizeWidth = 50f;
    public const float TileSizeHeight = 50f;

    private InventoryItem[,] _inventoryItemSlots;

    private RectTransform _rectTransform;

    [SerializeField] private int gridSizeWidth = 8;
    [SerializeField] private int gridSizeHeight = 5;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        InitGrid(gridSizeWidth, gridSizeHeight);
    }

    private void InitGrid(int width, int height)
    {
        _inventoryItemSlots = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * TileSizeWidth, height * TileSizeHeight);
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

        return _tileGridPosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY, ref InventoryItem overlapItem)
    {
        if (!BoundaryCheck(posX, posY, inventoryItem.Width, inventoryItem.Height))
        {
            return false;
        }

        if (!OverlapCheck(posX, posY, inventoryItem.Width, inventoryItem.Height, ref overlapItem))
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            ClearGrid(overlapItem);
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
                _inventoryItemSlots[posX + x, posY + y] = inventoryItem;
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
        position.x = posX * TileSizeWidth + TileSizeWidth * inventoryItem.Width / 2;
        position.y = -(posY * TileSizeHeight + TileSizeHeight * inventoryItem.Height / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
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

    public InventoryItem PickUpItem(int x, int y)
    {
        var toReturn = _inventoryItemSlots[x, y];

        if (toReturn == null) return null;

        ClearGrid(toReturn);

        return toReturn;
    }

    private void ClearGrid(InventoryItem cleanItem)
    {
        for (int x = 0; x < cleanItem.Width; x++)
        {
            for (int y = 0; y < cleanItem.Height; y++)
            {
                _inventoryItemSlots[cleanItem.onGridPosX + x, cleanItem.onGridPosY + y] = null;
            }
        }
    }

    public bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0)
        {
            return false;
        }

        if (posX >= gridSizeWidth || posY > gridSizeHeight)
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
                if (CheckAvailableSpace(x, y, itemToInsert.Width, itemToInsert.Height))
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return null;
    }
}
