using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton implementation, copied from Class 4 slides w/o implementation as a Generic
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();

                if (_instance == null)
                {
                    SoundManager obj = new GameObject().AddComponent<SoundManager>();
                    _instance = obj;
                }
            }
            
            return _instance;
        }
    }
    
    private AudioSource _source;
    [SerializeField] private AudioClip buttonPressedClip;
    
    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayButtonPressed()
    {
        _source.PlayOneShot(buttonPressedClip);
    }
}
