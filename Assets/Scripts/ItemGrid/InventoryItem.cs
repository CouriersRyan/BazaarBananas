using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

    public int Height
    {
        get
        {
            if (rotated)
            {
                return itemData.Width;
            }
            else
            {
                return itemData.Height;
            }
        }
    }

    public int Width
    {
        get
        {
            if (rotated)
            {
                return itemData.Height;
            }
            else
            {
                return itemData.Width;
            }
        }
    }

    public bool Size(int x, int y)
    {
        if (rotated)
        {
            return itemData.size[y, x];
        }
        else
        {
            return itemData.size[x, y];
        }
    }

    public int onGridPosX;
    public int onGridPosY;

    public bool rotated = false;

    private RectTransform _rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = GetComponent<RectTransform>();
                
            }
            return _rectTransform;
        }
        set
        {
            _rectTransform = value;
        }
    }
    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void Set(ItemData item)
    {
        itemData = item;

        GetComponent<Image>().sprite = itemData.itemIcon;
        
        var size = new Vector2();
        size.x = itemData.Width * ItemGrid.TileSizeWidth;
        size.y = itemData.Height * ItemGrid.TileSizeHeight;
        _rectTransform.sizeDelta = size;
    }

    public void Rotate()
    {
        rotated = !rotated;

        RectTransform.rotation = Quaternion.Euler(0, 0, rotated ? 90f : 0f);
    }
}
