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
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(on => Screen.fullScreen = on);
    }
}
