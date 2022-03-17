using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]
public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private InventoryController _inventoryController;
    private ItemGrid _itemGrid;

    private void Awake()
    {
        _inventoryController = GameManager.Instance.inventoryController;
        _itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inventoryController.selectedItemGrid = _itemGrid;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(_inventoryController.selectedItemGrid == _itemGrid) _inventoryController.selectedItemGrid = null;
    }
}
