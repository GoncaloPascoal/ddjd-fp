using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        fullscreenToggle.onValueChanged.AddListener(on =>
        {
            Screen.fullScreen = on;
            PlayerPrefs.SetInt("fullscreen", on ? 1 : 0);
        });
        fullscreenToggle.isOn = Screen.fullScreen;
    }
}
