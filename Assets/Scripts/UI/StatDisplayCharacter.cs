
using UnityEngine;
using UnityEngine.UI;

public class StatDisplayCharacter : StatDisplay
{
    [SerializeField] private MenuButtonStateSpriteDictionary sprites;
    [SerializeField] private Image button;
    
    public void SetState(MenuButtonState state)
    {
        button.sprite = sprites[state];
    }

    public void SetValue(float value, int points)
    {
        statValue.text = $"{value:F1} ({points})";
    }
}