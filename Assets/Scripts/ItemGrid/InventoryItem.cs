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
            if (rotation == Rotation.Angle90 || rotation == Rotation.Angle270)
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
            if (rotation == Rotation.Angle90 || rotation == Rotation.Angle270)

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
        switch (rotation)
        {
            case Rotation.Angle90:
                return itemData.size[itemData.size.arrays.Count - 1 - y, x];

            case Rotation.Angle180:
                return itemData.size[itemData.size.arrays.Count - 1 - x, itemData.size.arrays[0].cells.Count - 1 - y];

            case Rotation.Angle270:
                return itemData.size[y, itemData.size.arrays[0].cells.Count - 1 - x];
                break;
            
            case Rotation.Angle0:
                return itemData.size[x, y];
        }

        return false;
    }

    public int onGridPosX;
    public int onGridPosY;

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
    
    public Rotation rotation = Rotation.Angle0;
    
    public void Rotate()
    {
        rotation = (int)(rotation + 90) > 359 ? 0 : rotation + 90;
        
        
        
        RectTransform.rotation = Quaternion.Euler(0, 0, (int)(rotation));
    }
}

public enum Rotation
{
    Angle0 = 0,
    Angle90 = 90,
    Angle180 = 180,
    Angle270 = 270,
    NoAngle = -1
}
