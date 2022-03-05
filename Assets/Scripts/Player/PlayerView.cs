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
    [SerializeField] private TMP_Text protectionText;
    [SerializeField] private TMP_Text toolsText;
    [SerializeField] private TMP_Text foodText;
    
    //TODO Player can choose which resources to spend at events. (State)
    //     TODO Randomly generate resources needed to be spent and GUI.
    //     TODO Use SOs to set up this random events.
    //     TODO Maybe have a class for creating a generic event.
    //TODO use event binding to have the player return from a menu.
    
    // States: Select node -> move -> event/merchant -> repeat
    
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
        protectionText.text = model.Protection.ToString();
        toolsText.text = model.Tools.ToString();
        foodText.text = model.Food.ToString();
    }
    
    // Changes values of a designated trade resource.
    public void ChangeResource(TradeResources resource, int value)
    {
        switch (resource)
        {
            case TradeResources.Gold:
                model.Gold += value;
                break;
            
            case TradeResources.Food:
                model.Food += value;
                break;
            
            case TradeResources.Protection:
                model.Protection += value;
                break;
            
            case TradeResources.Tools:
                model.Tools += value;
                break;
        }
    }
    
    // Returns the value of a resource.
    public int GetResource(TradeResources resource)
    {
        switch (resource)
        {
            case TradeResources.Gold:
                return model.Gold;

            case TradeResources.Food:
                return model.Food;

            case TradeResources.Protection:
                return model.Protection;

            case TradeResources.Tools:
                return model.Tools;
        }

        return -1;
    }
    
    // Returns the current node the player is on.
    public Node GetCurrentNode()
    {
        return model.currentNode;
    }
}
