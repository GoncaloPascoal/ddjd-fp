using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class MenuTabGameObjectDictionary : SerializableDictionary<MenuTab, GameObject> { }

public class MenuTabController : MonoBehaviour
{
    [SerializeField] private MenuTabGameObjectDictionary tabs;
    [SerializeField] private MenuTab backToMenuTab;

    private bool _visible;
    private bool _inSubMenu;

    private int CurrentTab
    {
        get => _currentTab;
        set
        {
            tabs.ElementAt(_currentTab).Key.SetState(MenuButtonState.Inactive);
            _currentTab = value;
            tabs.ElementAt(_currentTab).Key.SetState(MenuButtonState.Active);
        }
    }
    private int _currentTab;

    private void Start()
    {
        _visible = false;
        _inSubMenu = false;
        _currentTab = 0;
    }

    private void Update()
    {
        if (InputManager.GetButtonDown("MenuToggle"))
        {
            ToggleMenu();
        }
        
        if (_inSubMenu) return;

        if (InputManager.GetButtonDown("MenuRight"))
        {
            CurrentTab = (CurrentTab + 1) % tabs.Count;
        }
        else if (InputManager.GetButtonDown("MenuLeft"))
        {
            if (CurrentTab == 0) CurrentTab = tabs.Count - 1;
            else CurrentTab -= 1;
        }
        else if (InputManager.GetButtonDown("MenuAction"))
        {
            SetTabSelected(true);
            _inSubMenu = true;
        }
    }

    private void ToggleMenu()
    {
        if (_visible) Return();

        _visible = !_visible;
        InputManager.CurrentActionType = _visible ? ActionType.Menu : ActionType.Game;
        transform.GetChild(0).gameObject.SetActive(_visible);
        if (_visible) tabs.ElementAt(CurrentTab).Key.SetState(MenuButtonState.Active);
    }

    private void SetTabSelected(bool selected)
    {
        KeyValuePair<MenuTab, GameObject> pair = tabs.ElementAt(CurrentTab);

        if (pair.Key == backToMenuTab)
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            pair.Key.SetState(selected ? MenuButtonState.Selected : MenuButtonState.Active);
            if (pair.Value != null) pair.Value.SetActive(selected);
        }
    }

    public void Return()
    {
        SetTabSelected(false);
        _inSubMenu = false;
    }
}
