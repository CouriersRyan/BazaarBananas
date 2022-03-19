using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class that handles the UI for selecting choices in an event.
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

    // Updates the information on the UI based on the values of the current node the player is on.
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
                    SoundManager.Instance.PlayButtonPressed();
                    if (player.CheckGold(eventGame.eventChoices[i1].Gold))
                    {
                        // Changes resources by the values in the choice. TODO: Fix this for new feature.
                        /*player.ChangeGold(TradeResources.Gold, eventGame.eventChoices[i1].Gold);
                        player.ChangeGold(TradeResources.Protection, eventGame.eventChoices[i1].Protection);
                        player.ChangeGold(TradeResources.Tools, eventGame.eventChoices[i1].Tools);
                        player.ChangeGold(TradeResources.Food, eventGame.eventChoices[i1].Food);*/
                        GameManager.Instance.FinishEvent(); // End the event once a choice has been made.
                    }
                });

                // Sets the main text.
                mainTexts[i].text = eventGame.eventChoices[i].Text;
                
                // Set the subext to be the cost of resources taken. Logic for applying commas and periods in appropriate places.
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
