 using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPreview : MonoBehaviour, ITradeResources
{
    private RectTransform _rectTransform;

    [SerializeField] private TMP_Text gold;
    [SerializeField] private TMP_Text protection;
    [SerializeField] private TMP_Text tools;
    [SerializeField] private TMP_Text food;
    [SerializeField] private TMP_Text value;
    [SerializeField] private TMP_Text otherText;
    

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public bool ShowPreview(TradeItem item, float posX, float posY)
    {
        Gold = item.Gold;
        Protection = item.Protection;
        Tools = item.Tools;
        Food = item.Food;
        Value = item.Value;
        OtherText = "";
        SetPos(posX, posY);
        return OverEdge();
    }

    public bool OverEdge()
    {
        var size = _rectTransform.sizeDelta;
        var pivot = _rectTransform.pivot;
        var up = (1 - pivot.y) * size.y;
        var down = (pivot.y) * size.y;
        var right = (1 - pivot.x) * size.x;
        var left = (pivot.x) * size.x;

        var isOverEdge = false;
        if (_rectTransform.position.y + up > 1080)
        {
            pivot.y += ((_rectTransform.position.y + up) - 1080) / size.y;
            isOverEdge = true;
        }
        if (_rectTransform.position.y - down < 0)
        {
            pivot.y += (_rectTransform.position.y - down) / size.y;
            isOverEdge = true;
        }
        if (_rectTransform.position.x + right > 1920)
        {
            pivot.x += ((_rectTransform.position.x + right) - 1920) / size.x;
            isOverEdge = true;
        }
        if (_rectTransform.position.y - left < 0)
        {
            pivot.x += (_rectTransform.position.x - left) / size.x;
            isOverEdge = true;
        }

        _rectTransform.pivot = pivot;
        return isOverEdge;
    }

    public void SetPos(float posX, float posY)
    {
        var pos = new Vector2(posX, posY);

        pos.x = pos.x > 1920 ? 1920 : posX;
        pos.x = pos.x < 0 ? 0 : posX;
        pos.y = pos.y > 1080 ? 1080 : posY;
        pos.y = pos.y < 0 ? 0 : posY;

        _rectTransform.position = pos;
    }
    
    public void SetPivot(float pivotX, float pivotY)
    {
        var pivot = new Vector2(pivotX, pivotY);
        _rectTransform.pivot = pivot;
    }

    public int Gold
    {
        get
        {
            return 0;
        }
        set
        {
            var text = "Premium: ";
            gold.text = String.Concat(text, value);
        }
    }

    public int Protection
    {
        get
        {
            return 0;
        }
        set
        {
            var text = "Protection: ";
            protection.text = String.Concat(text, value);
        }
    }

    public int Tools
    {
        get
        {
            return 0;
        }
        set
        {
            var text = "Tools: ";
            tools.text = String.Concat(text, value);
        }
    }

    public int Food
    {
        get
        {
            return 0;
        }
        set
        {
            var text = "Food: ";
            food.text = String.Concat(text, value);
        }
    }

    public int Value
    {
        get
        {
            return 0;
        }
        set
        {
            var text = "g";
            this.value.text = String.Concat(value, text);
        }
    }

    public string OtherText
    {
        get
        {
            return otherText.text;
        }
        set
        {
            otherText.text = value;
        }
    }

    public Vector2 Pivot
    {
        get
        {
            return _rectTransform.pivot;
        }
    }
    public Vector2 Size
    {
        get
        {
            return _rectTransform.sizeDelta;
        }
    }
    
    public Vector2 Pos
    {
        get
        {
            return _rectTransform.position;
        }
    }
}
