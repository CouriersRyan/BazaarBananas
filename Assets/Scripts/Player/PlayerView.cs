using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The View portion of the Player MVC.
// The MonoBehavior that exists in the scene.
public class PlayerView : MonoBehaviour
{
    // FSM
    [SerializeField] private PlayerModel model;
    private PlayerController _controller;
    
    //TODO Player can buy and sell resources on the market. (State)
    //     TODO Randomly generate exchange rates and GUI for displaying it. 
    //TODO Player can choose which resources to spend at events. (State)
    //     TODO Randomly generate resources needed to be spent and GUI.
    
    // States: Select node -> move -> event/merchant -> repeat
    
    void Start()
    {
        _controller = new PlayerController(model);
    }

    // Updates the game every frame using logic from the controller.
    void Update()
    {
        _controller.Update();
    }
}
