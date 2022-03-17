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

    public bool PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        if (!BoundaryCheck(posX, posY, inventoryItem.itemData.width, inventoryItem.itemData.height))
        {
            return false;
        }
        
        RectTransform rectTransform = inventoryItem.RectTransform;
        rectTransform.SetParent(_rectTransform);

        for (int x = 0; x < inventoryItem.itemData.width; x++)
        {
            for (int y = 0; y < inventoryItem.itemData.height; y++)
            {
                _inventoryItemSlots[posX + x, posY + y] = inventoryItem;
            }
        }

        inventoryItem.onGridPosX = posX;
        inventoryItem.onGridPosY = posY;

        Vector2 position = new Vector2();
        position.x = posX * TileSizeWidth + TileSizeWidth * inventoryItem.itemData.width / 2;
        position.y = -(posY * TileSizeHeight + TileSizeHeight * inventoryItem.itemData.height / 2);

        rectTransform.localPosition = position;

        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        var toReturn = _inventoryItemSlots[x, y];

        if (toReturn == null) return null;

        for (int xi = 0; xi < toReturn.itemData.width; xi++)
        {
            for (int yi = 0; yi < toReturn.itemData.height; yi++)
            {
                _inventoryItemSlots[toReturn.onGridPosX + xi, toReturn.onGridPosY + yi] = null;
            }
        }

        return toReturn;
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
}
