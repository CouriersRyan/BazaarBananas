using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                
                DontDestroyOnLoad(_instance);
            }
            
            return _instance;
        }
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
}
