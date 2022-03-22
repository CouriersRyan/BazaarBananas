using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGameManager : MonoBehaviour
{
    
    // Singleton implementation, copied from Class 4 slides w/o implementation as a Generic
    private static EventGameManager _instance;

    public static EventGameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EventGameManager>();

                if (_instance == null)
                {
                    var obj = new GameObject().AddComponent<EventGameManager>();
                    _instance = obj;
                }
            }
            
            return _instance;
        }
    }
    
    [SerializeField] private EventData[] events;
    [SerializeField] private EventData[] tributeEvents;

    public EventData[] GetEvents(bool isMarket)
    {
        if (isMarket)
        {
            return tributeEvents;
        }
        return events;
    }

    public EventData GetRandomEvent(bool isMarket)
    {
        if (isMarket)
        {
            return tributeEvents[Random.Range(0, tributeEvents.Length - 1)];
        }
        else
        {
            return events[Random.Range(0, events.Length - 1)];
        }
    }
}
