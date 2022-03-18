using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryController))]
public class MarketGrid : MonoBehaviour
{
    private InventoryController _controller;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<InventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
