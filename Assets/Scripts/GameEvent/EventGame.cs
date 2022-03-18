using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that is used to determine the prompts and choices available in an event.
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

    // Creates random values for an event.
    // Theoretically, if I kept working on this project, I would use SOs to make a bunch of possible scripted events
    // to pull from, but I do not have the time to do that for now.
    public void RandomizeEvent()
    {
        // Default prompt.
        prompt = "You have encountered an event! Spend resources to try and get past it.";

        var numChoices = Random.Range(2, eventChoices.Length + 1); // Chooses a number of choices the event has, between 2 and 4.
        
        // Loops through and sets values for each event choice.
        for (int i = 0; i < eventChoices.Length; i++)
        {
            eventChoices[i].isActiveChoice = false;
            var goldCost = Random.Range(-50, 25);
            var protectionCost = Random.Range(-6, 3);
            var toolsCost = Random.Range(-6, 3);
            var foodCost = Random.Range(-6, 3);
            if (numChoices > 0) // Only applies and makes the choice active if the number of choices has not already been exceeded.
            {
                eventChoices[i] = new EventChoice("This is choice number " + (i + 1), goldCost, protectionCost, toolsCost, foodCost);
                numChoices--;
            }
        }
    }
}
