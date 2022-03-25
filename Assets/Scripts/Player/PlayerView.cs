using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;

// The View portion of the Player MVC.
// The MonoBehavior that exists in the scene.
public class PlayerView : MonoBehaviour
{
    // FSM
    [SerializeField] private PlayerModel model;
    private PlayerController _controller;
    
    // UI References
    [SerializeField] private TMP_Text goldText;

    void Start()
    {
        _controller = new PlayerController(model);
        GameManager.Instance.m_OffEvent.AddListener(EndStateEvent);
        SetState(stateEvent);
    }

    // Updates the game every frame using logic from the controller.
    void Update()
    {
        State.StateUpdate(_controller, this);
        UpdateUI();
    }
    
    // Player FSM
    [NonSerialized] public readonly PlayerStateBase stateSelect = new PlayerStateSelect();
    [NonSerialized] public readonly PlayerStateBase stateMove = new PlayerStateMove();
    [NonSerialized] public readonly PlayerStateBase stateEvent = new PlayerStateEvent();
    [NonSerialized] private PlayerStateBase state;

    public PlayerStateBase State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }
    
    // Sets the state in the FSM.
    public void SetState(PlayerStateBase newState)
    {
        if (State != null)
        {
            State.StateExit(_controller, this);
        }

        State = newState;

        if (State != null)
        {
            State.StateEnter(_controller, this);
        }
    }
    
    // Sets the state from PlayerStateEvent, which is triggered by a button.
    public void EndStateEvent()
    {
        if (State == stateEvent)
        {
            SetState(stateSelect);
        }
    }
    
    // Updates all UI elements with values from the model
    public void UpdateUI()
    {
        goldText.text = model.Gold.ToString() + "g";
    }
    
    // Changes values of a designated trade resource.
    public void ChangeGold(int value)
    {
        model.Gold += value;
    }
    
    // Returns the value of a resource.
    public int GetGold()
    {
        return model.Gold;
    }

    public int[] GetResources()
    {
        int[] resourceTotals = new int[4];
        var items = model.playerInventory.GetItemsInGrid();
        foreach (var item in items)
        {
            resourceTotals[(int)TradeResources.Gold] += ((InventoryTradeItem)item).tradeItem.Gold;
            resourceTotals[(int)TradeResources.Protection] += ((InventoryTradeItem)item).tradeItem.Protection;
            resourceTotals[(int)TradeResources.Tools] += ((InventoryTradeItem)item).tradeItem.Tools;
            resourceTotals[(int)TradeResources.Food] += ((InventoryTradeItem)item).tradeItem.Food;
        }

        return resourceTotals;
    }

    // Check if the resources can be changed by a certain value and remain nonnegative.
    public bool CheckGold(int goldDelta)
    {
        var isValid = true;
        isValid = !(model.Gold < -goldDelta) && isValid;
        return isValid;
    }
    
    // Returns the current node the player is on.
    public Node GetCurrentNode()
    {
        return model.currentNode;
    }
    
    // Returns the player's inventory item grid
    public ItemGrid GetInventory()
    {
        return model.playerInventory;
    }
}
