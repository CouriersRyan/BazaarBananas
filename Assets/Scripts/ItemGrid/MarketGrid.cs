using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles interactions between the inventory system and the rest of the game systems.
[RequireComponent(typeof(InventoryController))]
public class MarketGrid : MonoBehaviour
{
    private InventoryController _controller;
    private PlayerView _player;
    private BargainingMarket _market;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<InventoryController>();
        _player = GameManager.Instance.GetPlayer();
    }

    private void CreateItem()
    {
        
    }
    
}
