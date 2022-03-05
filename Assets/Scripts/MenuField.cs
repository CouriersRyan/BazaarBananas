using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// A component that holds all the functions buttons in the market and event need to call.
public class MenuField : MonoBehaviour
{
    [SerializeField] private TradeResources[] resourcesInField = new TradeResources[2];

    [SerializeField] private TMP_Text resourceText0;
    [SerializeField] private TMP_Text resourceText1;

    private PlayerView player;
    private MapNode node;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        GameManager.Instance.m_OnEvent.AddListener(UpdateUI);
    }

    // Decreases the first resource and increases the second based on value in the node's market.
    public void ConvertForward()
    {
        node = player.GetCurrentNode().Obj.GetComponent<MapNode>();
        if (node != null)
        {
            var value = node.Market.GetConversion(resourcesInField[0], resourcesInField[1]);
            if (player.GetResource(resourcesInField[0]) >= value)
            {
                player.ChangeResource(resourcesInField[0], -value);
                player.ChangeResource(resourcesInField[1], 1);
                player.UpdateUI();
            }
        }
    }
    
    // Decreases the first resource and increases the second based on value in the node's market.
    public void ConvertBackward()
    {
        node = player.GetCurrentNode().Obj.GetComponent<MapNode>();
        if (node != null)
        {
            var value = node.Market.GetConversion(resourcesInField[0], resourcesInField[1]);
            if (player.GetResource(resourcesInField[1]) >= 1)
            {
                player.ChangeResource(resourcesInField[0], value);
                player.ChangeResource(resourcesInField[1], -1);
                player.UpdateUI();
            }
        }
    }

    public void UpdateUI()
    {
        node = player.GetCurrentNode().Obj.GetComponent<MapNode>();
        var value = node.Market.GetConversion(resourcesInField[0], resourcesInField[1]);
        resourceText0.text = value.ToString();
        resourceText1.text = "1";
        Debug.Log("here");
    }
}
