using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TMP_Text levelText, progressText, statPointsText;

    private int CurrentStat
    {
        get => _currentStat;
        set
        {
            if (LevelSystem.Instance.StatPointsRemaining > 0)
            {
                _statDisplays[_currentStat].SetState(MenuButtonState.Active);
                _statDisplays[value].SetState(MenuButtonState.Selected);
            }
            _currentStat = value;
        }
    }

    private List<StatDisplayCharacter> _statDisplays;
    private int _currentStat = 0;

    private MenuTabController _menuTabController;
    private StatsPlayer _stats;

    private void Start()
    {
        _menuTabController = GetComponentInParent<MenuTabController>();
        _statDisplays = GetComponentsInChildren<StatDisplayCharacter>().ToList();
        _stats = GameObject.FindWithTag("Player").GetComponent<StatsPlayer>();
        UpdateInterface();
    }

    private void OnEnable()
    {
        if (_menuTabController != null) UpdateInterface();
    }

    private void UpdateInterface()
    {
        UpdateLevelInformation();
        UpdateStats();
    }

    private void UpdateLevelInformation()
    {
        levelText.text = LevelSystem.Instance.Level.ToString();
        int experienceToLevelUp = LevelSystem.Instance.ExperienceToLevelUp();
        progressText.text = $"{LevelSystem.Instance.Experience} / {experienceToLevelUp}";
        experienceSlider.value = (float) LevelSystem.Instance.Experience / experienceToLevelUp;
    }

    private void UpdateStats()
    {
        statPointsText.text = LevelSystem.Instance.StatPointsRemaining.ToString();
        bool hasStatPoints = LevelSystem.Instance.HasStatPoints();

        foreach (StatDisplayCharacter display in _statDisplays)
        {
            display.SetState(hasStatPoints ? MenuButtonState.Active : MenuButtonState.Inactive);
            display.SetValue(_stats.GetStatValue(display.stat), LevelSystem.Instance.GetStatPoints(display.stat));
        }
        _statDisplays[_currentStat].SetState(hasStatPoints ? MenuButtonState.Selected : MenuButtonState.Inactive);
    }

    private void Update()
    {
        if (InputManager.GetButtonDown("MenuBack"))
        {
            _menuTabController.Return();
            return;
        }

        if (InputManager.GetButtonDown("MenuDown"))
        {
            CurrentStat = (CurrentStat + 1) % _statDisplays.Count;
        }
        else if (InputManager.GetButtonDown("MenuUp"))
        {
            if (CurrentStat == 0) CurrentStat = _statDisplays.Count - 1;
            else CurrentStat -= 1;
        }
        else if (InputManager.GetButtonDown("MenuAction"))
        {
            if (LevelSystem.Instance.HasStatPoints())
            {
                LevelSystem.Instance.AddStatPoint(_statDisplays[_currentStat].stat);
                UpdateStats();
            }
        }
    }
}
