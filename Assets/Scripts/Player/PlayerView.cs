using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private PlayerModel model;
    private PlayerController _controller;
    
    //TODO Player can click on the next node and move to that node. (State)
    //     TODO Refer to map of nodes to know which node to move to next.
    //TODO Player can buy and sell resources on the market. (State)
    //     TODO Randomly generate exchange rates and GUI for displaying it. 
    //TODO Player can choose which resources to spend at events. (State)
    //     TODO Randomly generate resources needed to be spent and GUI.
    
    // States: Select node -> move -> event/merchant -> repeat
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = new PlayerController(model);
    }

    // Update is called once per frame
    void Update()
    {
        _controller.Update();
    }
}
