using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] public Bar healthBar, staminaBar;
    [SerializeField] private GameObject buttonPrompt;
    [SerializeField] private ItemPickupDisplay itemPickup;

    public static HUD Instance;

    private TMP_Text _buttonPromptText;
    private readonly Stack<string> _buttonPrompts = new Stack<string>();

    private void Awake()
    {
        Instance = this;
        _buttonPromptText = buttonPrompt.GetComponentInChildren<TMP_Text>();
    }

    public void ShowButtonPrompt(IEnumerable<string> buttons, string action)
    {
        string bStr = string.Join(", ", buttons);
        _buttonPrompts.Push($"{bStr}: {action}");
        UpdateButtonPrompt();
    }

    public void HideButtonPrompt()
    {
        _buttonPrompts.Pop();
        UpdateButtonPrompt();
    }

    private void UpdateButtonPrompt()
    {
        if (_buttonPrompts.Count == 0)
        {
            buttonPrompt.SetActive(false);
        }
        else
        {
            buttonPrompt.SetActive(true);
            _buttonPromptText.SetText(_buttonPrompts.Peek());
        }
    }

    public void ShowItemPickup(ItemPickupDictionary items, float duration = 4f)
    {
        itemPickup.ShowItemPickup(items, duration);
    }
}
