using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class that is used to determine the prompts and choices available in an event.
public class EventGame : MonoBehaviour
{
    // An array of up to four choices per event that can change any of the four resources.
    public EventData eventData;

    // Creates random values for an event.
    // Theoretically, if I kept working on this project, I would use SOs to make a bunch of possible scripted events
    // to pull from, but I do not have the time to do that for now.
    public void RandomizeEvent(bool isMarket)
    {
        eventData = EventGameManager.Instance.GetRandomEvent(isMarket);
    }
}
