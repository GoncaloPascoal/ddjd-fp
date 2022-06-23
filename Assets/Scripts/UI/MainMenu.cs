using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton, loadGameButton, settingsButton, quitButton;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        settingsButton.onClick.AddListener(() => settingsMenu.SetActive(true));
        quitButton.onClick.AddListener(Application.Quit);
    }
}
