using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] private RectTransform highlighterContainer;
    [SerializeField] private GameObject highlighterPrefab;

    private List<GameObject> _highlightPool = new List<GameObject>();
    private InventoryItem _currentItem;

    public void Show(bool b)
    {
        if (!b)
        {
            HideHightlights();

            _currentItem = null;
        }

        highlighterContainer.gameObject.SetActive(b);
    }

    private void HideHightlights()
    {
        foreach (var highlight in _highlightPool)
        {
            highlight.gameObject.SetActive(false);
        }
    }

    public void SetSize(InventoryItem item)
    {
        if (_currentItem != item)
        {
            _currentItem = item;
            HideHightlights();
            for (int x = 0; x < item.Width; x++)
            {
                for (int y = 0; y < item.Height; y++)
                {
                    if (item.Size(x, y))
                    {
                        var highlight = SpawnHighlighterFromPool();
                        highlight.transform.localPosition = new Vector2(
                            (-item.Width * ItemGrid.TileSizeWidthUnscaled / 2) + x * ItemGrid.TileSizeWidthUnscaled + (ItemGrid.TileSizeWidthUnscaled/2),
                            (item.Height * ItemGrid.TileSizeHeightUnscaled / 2) - (y * ItemGrid.TileSizeHeightUnscaled) - (ItemGrid.TileSizeHeightUnscaled/2));
                    }
                }
            }
        }
        //highlighterContainer.sizeDelta = size;
    }

    // Return an inactive highlight from the pool, if there are none available then instead
    private GameObject SpawnHighlighterFromPool()
    {
        foreach (var highlight in _highlightPool)
        {
            if (!highlight.gameObject.activeInHierarchy)
            {
                highlight.gameObject.SetActive(true);
                return highlight;
            }
        }

        var temp = Instantiate(highlighterPrefab, transform.position, Quaternion.identity);
        _highlightPool.Add(temp);
        temp.transform.SetParent(highlighterContainer);
        temp.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemGrid.TileSizeWidth, ItemGrid.TileSizeHeight);
        return temp;
    }

    public void AssignParent(ItemGrid grid)
    {
        if (grid == null)
        {
            return;
        }

        highlighterContainer.SetParent(grid.transform);
        highlighterContainer.SetAsLastSibling();
    }

    public void SetPosition(ItemGrid grid, InventoryItem item)
    {
        Vector2 pos = grid.CalcPosition(item, item.onGridPosX, item.onGridPosY);

        highlighterContainer.localPosition = pos;
    }

    public void SetPosition(ItemGrid grid, InventoryItem item, int posX, int posY)
    {
        Vector2 pos = grid.CalcPosition(item, posX, posY);

        highlighterContainer.localPosition = pos;
    }
}