using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    private Node node;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetNode(Node node)
    {
        this.node = node;
    }
    
    public Node GetNode()
    {
        return node;
    }
}
