using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button newGameButton, loadGameButton, settingsButton, quitButton;
    [SerializeField] private GameObject settingsMenu;

    private void Start()
    {
        if (PlayerPrefs.HasKey("resolutionX") && PlayerPrefs.HasKey("resolutionY"))
        {
            Screen.SetResolution(PlayerPrefs.GetInt("resolutionX"), PlayerPrefs.GetInt("resolutionY"), Screen.fullScreen);
        }
        Screen.fullScreen = PlayerPrefs.GetInt("fullscreen", 0) == 1;

        newGameButton.onClick.AddListener(() => SceneManager.LoadScene("Level1"));
        settingsButton.onClick.AddListener(() =>
        {
            PlayerPrefs.Save();
            settingsMenu.SetActive(true);
        });
        quitButton.onClick.AddListener(Application.Quit);
    }
}
