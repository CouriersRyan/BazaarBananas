using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITradeResources
{
    int Gold { get; set; } // Gold
    int Protection { get; set; } // Red
    int Tools { get; set; } // Blue
    int Food { get; set; } // Green
}
