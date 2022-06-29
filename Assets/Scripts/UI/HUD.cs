using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] public Bar healthBar, staminaBar;
    [SerializeField] private GameObject buttonPrompt, messageDisplay;
    [SerializeField] private ItemPickupDisplay itemPickup;

    public static HUD Instance;

    private TMP_Text _buttonPromptText, _messageDisplayText;
    private readonly Stack<string> _buttonPrompts = new Stack<string>(),
        _messages = new Stack<string>();

    private void Awake()
    {
        Instance = this;
        _buttonPromptText = buttonPrompt.GetComponentInChildren<TMP_Text>();
        _messageDisplayText = messageDisplay.GetComponentInChildren<TMP_Text>();
    }

    public void ShowButtonPrompt(IEnumerable<string> buttons, string action)
    {
        string bStr = string.Join(", ", buttons);
        ShowButtonPromptRaw($"{bStr}: {action}");
    }

    public void ShowButtonPromptRaw(string text)
    {
        _buttonPrompts.Push(text);
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

    public void ShowMessage(string message)
    {
        _messages.Push(message);
        UpdateMessage();
    }

    public void HideMessage()
    {
        _messages.Pop();
        UpdateMessage();
    }

    private void UpdateMessage()
    {
        if (_messages.Count == 0)
        {
            messageDisplay.SetActive(false);
        }
        else
        {
            messageDisplay.SetActive(true);
            _messageDisplayText.SetText(_messages.Peek());
        }
    }
}
