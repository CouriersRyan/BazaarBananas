using System.Collections;
using System.Collections.Generic;
<<<<<<< HEAD
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EventMenu : MonoBehaviour
{
    private static int fields = 4;
    [SerializeField] private Button[] buttons = new Button[fields];

    [SerializeField] private TMP_Text[] mainTexts = new TMP_Text[fields];

    [SerializeField] private TMP_Text[] subTexts = new TMP_Text[fields];

    [SerializeField] private TMP_Text prompt;
    
    private PlayerView player;
    private MapNode node;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        GameManager.Instance.m_OnEvent.AddListener(UpdateUI);
    }

    public void UpdateUI()
    {

        node = player.GetCurrentNode().Obj.GetComponent<MapNode>();
        var eventGame = node.EventGame;

        prompt.text = eventGame.prompt;
        
        for (int i = 0; i < fields; i++)
        {
            // Clear previous button functionality and adds a new prompt for onClick.
            buttons[i].onClick.RemoveAllListeners();
            if (eventGame.eventChoices[i].isActiveChoice)
            {
                buttons[i].gameObject.SetActive(true);
                var i1 = i; //For the anonymous function, the value does not change.
                
                //Uses an anonymous function to assign actions to the button when clicked.
                buttons[i].onClick.AddListener(() =>
                {
                    //Check if any value would go below zero from choosing this option and if none will, then allow
                    //the player to pick it.
                    if (player.CheckResources(eventGame.eventChoices[i1].Gold,
                            eventGame.eventChoices[i1].Protection, eventGame.eventChoices[i1].Tools,
                            eventGame.eventChoices[i1].Food))
                    {
                        // Changes resources by the values in the choice.
                        player.ChangeResource(TradeResources.Gold, eventGame.eventChoices[i1].Gold);
                        player.ChangeResource(TradeResources.Protection, eventGame.eventChoices[i1].Protection);
                        player.ChangeResource(TradeResources.Tools, eventGame.eventChoices[i1].Tools);
                        player.ChangeResource(TradeResources.Food, eventGame.eventChoices[i1].Food);
                        GameManager.Instance.FinishEvent(); // End the event once a choice has been made.
                    }
                });

                // Sets the main text.
                mainTexts[i].text = eventGame.eventChoices[i].Text;
                
                // Set the subext to be the cost of resources taken.
                var subText = "";
                var hasPrevResource = false;
                if (eventGame.eventChoices[i].Gold != 0)
                {
                    subText += "Gold: " + eventGame.eventChoices[i].Gold;
                    hasPrevResource = true;
                }
                if (eventGame.eventChoices[i].Protection != 0)
                {
                    if (hasPrevResource) subText += ", ";
                    subText += "Protection: " + eventGame.eventChoices[i].Protection;
                    hasPrevResource = true;
                }
                if (eventGame.eventChoices[i].Tools != 0)
                {
                    if (hasPrevResource) subText += ", ";
                    subText += "Tools: " + eventGame.eventChoices[i].Tools;
                    hasPrevResource = true;
                }
                if (eventGame.eventChoices[i].Food != 0)
                {
                    if (hasPrevResource) subText += ", ";
                    subText += "Food: " + eventGame.eventChoices[i].Food;
                }
                subText += ".";

                subTexts[i].text = subText;
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
        }
    }


}
