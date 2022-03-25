using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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

    // Unity event from opening and closing an event or market, and ending the game.
    public UnityEvent m_OnEvent;
    public UnityEvent m_OffEvent;
    public UnityEvent m_OnMarket;
    public UnityEvent m_EndGame;
    public UnityEvent m_GameOver;
    
    // Defines the events and adds listeners from the Game Manager.
    private void Awake()
    {
        if (m_OnEvent == null)
        {
            m_OnEvent = new UnityEvent();
        }
        m_OnEvent.AddListener(ToggleMenu);
        m_OnEvent.AddListener(OpenEvent);
        m_OnEvent.AddListener(CheckForLastNode);
        
        if (m_OffEvent == null)
        {
            m_OffEvent = new UnityEvent();
        }
        m_OffEvent.AddListener(ToggleMenu);

        if (m_EndGame == null)
        {
            m_EndGame = new UnityEvent();
        }
        m_EndGame.AddListener(DisplayResults);
        m_EndGame.AddListener(CalculateScore);

        if (m_OnMarket == null)
        {
            m_OnMarket = new UnityEvent();
        }

        marketCamera.enabled = false;
        overviewCamera.enabled = true;

        if (m_GameOver == null)
        {
            m_GameOver = new UnityEvent();
        }
        m_GameOver.AddListener(GameOver);
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

    // References to the menus the player will be interacting with in the game.
    [SerializeField] private GameObject toggleMenu;
    [SerializeField] private GameObject marketMenu;
    [SerializeField] private GameObject eventMenu;
    [SerializeField] private GameObject resultScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject GUI;
    [SerializeField] private GameObject toggleGrid;
    [SerializeField] private GameObject marketGrid;
    [SerializeField] private GameObject eventGrid;
    [SerializeField] private GameObject playerGrid;

    [SerializeField] private TMP_Text[] bananas = new TMP_Text[5];
    [SerializeField] private TMP_Text[] resources = new TMP_Text[4];
    
    [SerializeField] public InventoryController inventoryController;

    //Open/Closes the current menu based on the node.
    public void ToggleMenu()
    {
        toggleMenu.SetActive(!toggleMenu.activeInHierarchy);
        toggleGrid.SetActive(!toggleGrid.activeInHierarchy); 
        SwitchCamera();
    }

    public void OpenEvent()
    {
        eventMenu.SetActive(true);
        eventGrid.SetActive(true);
        playerGrid.SetActive(true);
        marketMenu.SetActive(false);
        marketGrid.SetActive(false);
    }

    public void OpenMarket()
    {
        eventMenu.SetActive(false);
        eventGrid.SetActive(false);
        playerGrid.SetActive(true);
        marketMenu.SetActive(true);
        marketGrid.SetActive(true);
        m_OnMarket.Invoke();
    }

    //Displays the results screen and hides all other screens.
    public void DisplayResults()
    {
        resultScreen.SetActive(true);
        GUI.SetActive(false);

    }
    
    //Calculates the score and set it on the end screen.
    public void CalculateScore()
    {

        var total = 0;
        
        bananas[0].text = _player.GetGold().ToString();
        resources[0].text = _player.GetGold().ToString();
        total += _player.GetGold();

        var resourceTotals = _player.GetResources();
        
        for (int i = 1; i < resources.Length; i++)
        {
            bananas[i].text = resourceTotals[i].ToString();
            resources[i].text = resourceTotals[i].ToString();
        }

        bananas[^1].text = total.ToString();
    }
    
    //Check if the node the player is on is the last one, if it is end the game.
    public void CheckForLastNode()
    {
        if (_player.GetCurrentNode() == _map.FindNorthmostNode())
        {
            m_EndGame.Invoke();
        }
    }
    
    // Ends the current event or market.
    public void FinishEvent()
    {
        m_OffEvent.Invoke();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    [SerializeField] private Camera overviewCamera;
    [SerializeField] private Camera marketCamera;

    public void SwitchCamera()
    {
        overviewCamera.enabled = !overviewCamera.enabled;
        marketCamera.enabled = !marketCamera.enabled;
    }

    // Runs when the player fails a condition.
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        GUI.SetActive(false);
    }
}
