using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemData itemData;

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
        size.x = itemData.width * ItemGrid.TileSizeWidth;
        size.y = itemData.height * ItemGrid.TileSizeHeight;
        _rectTransform.sizeDelta = size;
    }
}
