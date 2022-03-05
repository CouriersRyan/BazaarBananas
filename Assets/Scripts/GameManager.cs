using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Game Manager Singleton to hold global variables.
public class GameManager : MonoBehaviour
{
    // Singleton implementation, copied from Class 4 slides w/o implementation as a Generic
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameManager obj = new GameObject().AddComponent<GameManager>();
                    _instance = obj;
                }
            }
            
            return _instance;
        }
    }

    public UnityEvent m_OnEvent;
    public UnityEvent m_OffEvent;
    
    private void Awake()
    {
        if (m_OnEvent == null)
        {
            m_OnEvent = new UnityEvent();
        }
        m_OnEvent.AddListener(ToggleMenu);

        if (m_OffEvent == null)
        {
            m_OffEvent = new UnityEvent();
        }
        m_OffEvent.AddListener(ToggleMenu);
    }

    private Map _map; //Reference to the map in the scene.

    public Map GetMap()
    {
        if (_map == null)
        {
            _map = FindObjectOfType<Map>();
        }

        return _map;
    }

    private PlayerView _player; //Reference to the player in the scene.
    
    public PlayerView GetPlayer()
    {
        if (_player == null)
        {
            _player = FindObjectOfType<PlayerView>();
        }

        return _player;
    }

    // References to the two menus the player will be interacting with in the game.
    [SerializeField] private GameObject marketMenu;
    [SerializeField] private GameObject eventMenu;
    
    //Open/Closes the current menu based on the node.
    public void ToggleMenu()
    {
        if (GetPlayer().GetCurrentNode().Obj.GetComponent<MapNode>().IsMarket)
        {
            marketMenu.SetActive(!marketMenu.activeInHierarchy);
        }
        else
        {
            eventMenu.SetActive(!eventMenu.activeInHierarchy);
        }
    }
    
    // Ends the current event or market.
    public void FinishEvent()
    {
        m_OffEvent.Invoke();
    }
}
