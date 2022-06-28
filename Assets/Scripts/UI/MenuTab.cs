using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuButtonState
{
    Inactive,
    Active,
    Selected,
}

[Serializable]
public class MenuButtonStateSpriteDictionary : SerializableDictionary<MenuButtonState, Sprite> { }

public class MenuTab : MonoBehaviour
{
    [SerializeField] private MenuButtonStateSpriteDictionary sprites;

    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetState(MenuButtonState state)
    {
        _image.sprite = sprites[state];
    }
}
