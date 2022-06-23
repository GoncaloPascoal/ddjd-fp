using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionToggle : MonoBehaviour
{
    [SerializeField] private Vector2Int resolution;
    [SerializeField] private TMP_Text text;

    private void Start()
    {
        text.text = $"{resolution.x} x {resolution.y}";

        Toggle toggle = GetComponent<Toggle>();
        toggle.isOn = Screen.currentResolution.width == resolution.x && Screen.currentResolution.height == resolution.y;
        toggle.onValueChanged.AddListener(on =>
        {
            if (on) Screen.SetResolution(resolution.x, resolution.y, Screen.fullScreen);
        });
    }
}
