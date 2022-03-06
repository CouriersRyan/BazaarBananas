using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Stores all the methods used by buttons in the Main Menu
public class MainMenu : MonoBehaviour
{
    public void GoToNextScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
