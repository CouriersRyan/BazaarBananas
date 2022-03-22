using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform highlighter;


    public void Show(bool b)
    {
        highlighter.gameObject.SetActive(b);
    }
    
    public void SetSize(InventoryItem item)
    {
        Vector2 size = new Vector2();
        size.x = item.Width * ItemGrid.TileSizeWidthUnscaled;
        size.y = item.Height * ItemGrid.TileSizeHeightUnscaled;
        highlighter.sizeDelta = size;
    }

    public void AssignParent(ItemGrid grid)
    {
        if (grid == null) {return;}
        highlighter.SetParent(grid.transform);
        highlighter.SetAsLastSibling();
    }
    
    public void SetPosition(ItemGrid grid, InventoryItem item)
    {
        Vector2 pos = grid.CalcPosition(item, item.onGridPosX, item.onGridPosY);

        highlighter.localPosition = pos;
    }
    public void SetPosition(ItemGrid grid, InventoryItem item, int posX, int posY)
    {
        Vector2 pos = grid.CalcPosition(item, posX, posY);

        highlighter.localPosition = pos;
    }
}
