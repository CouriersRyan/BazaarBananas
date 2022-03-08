using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A struct that stores all information for a choice in an Event.
public struct EventChoice : ITradeResources
{
    public string Text { get; set; }
    public int Gold { get; set; }
    public int Protection { get; set; }
    public int Tools { get; set; }
    public int Food { get; set; }

    
    // Should data in this current choice be pulled from.
    public bool isActiveChoice { get; set; }
    
    // Constructor
    public EventChoice(string text, int gold, int protection, int tools, int food)
    {
        Text = text;
        Gold = gold;
        Protection = protection;
        Tools = tools;
        Food = food;
        isActiveChoice = true;
    }
}
