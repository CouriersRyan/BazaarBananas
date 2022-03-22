using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "ScriptableObjects/EventData", order = 2)]
public class EventData : ScriptableObject
{
    public string prompt;
    public string result;
    
    //Required stats to fulfill to pass the event.
    public int value;
    public int gold;
    public int protection;
    public int tools;
    public int food;

    public Penalty penalty;

    public ItemData[] reward;
}
