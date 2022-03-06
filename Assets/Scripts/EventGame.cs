using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class EventGame : MonoBehaviour
{
    public string prompt;
    
    // An array of up to four choices per event that can change any of the four resources.
    public EventChoice[] eventChoices = new EventChoice[4];


    // When created, class generates a prompt with a series of choices.
    void Awake()
    {
        RandomizeEvent();
    }

    //Either pulls from a random event available or uses a premade SO for an event.
    public void RandomizeEvent()
    {
        // Random chance to pull from an SO.
        
        //Default Random prompt.
        prompt = "You have encountered an event! Spend resources to try and get past it.";

        var numChoices = Random.Range(0, eventChoices.Length);
        
        
        for (int i = 0; i < eventChoices.Length; i++)
        {
            eventChoices[i].isActiveChoice = false;
            var goldCost = Random.Range(-50, 25);
            var protectionCost = Random.Range(-6, 3);
            var toolsCost = Random.Range(-6, 3);
            var foodCost = Random.Range(-6, 3);
            if (numChoices > 0)
            {
                eventChoices[i] = new EventChoice("This is choice number " + (i + 1), goldCost, protectionCost, toolsCost, foodCost);
                numChoices--;
            }
        }
    }
}
