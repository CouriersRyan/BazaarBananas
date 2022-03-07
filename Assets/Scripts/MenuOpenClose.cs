using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Class that handles buttons for opening and closing menus.
public class MenuOpenClose : MonoBehaviour
{
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject close;

    private void Start()
    {
        GameManager.Instance.m_OnEvent.AddListener(ToggleClose);
        GameManager.Instance.m_OffEvent.AddListener(ToggleClose);
    }

    // For the buttons, clicking open/close button will run this method.
    public void ToggleMenu()
    {
        GameManager.Instance.ToggleMenu();
        ToggleOpen();
        ToggleClose();
        SoundManager.Instance.PlayButtonPressed();
    }

    //Toggles the button that would open the menu.
    public void ToggleOpen()
    {
        open.SetActive(!open.activeInHierarchy);
    }

    //Toggles the button that would close the menu.
    public void ToggleClose()
    {
        close.SetActive(!close.activeInHierarchy);
    }
}
