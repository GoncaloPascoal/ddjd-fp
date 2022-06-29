using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChanger : MonoBehaviour
{
    private Animator _animator;

    private int _nextSceneIndex;

    private GameObject _healthBar;
    private GameObject _staminaBar;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void ChangeLevel(int levelIndex)
    {
        _nextSceneIndex = levelIndex;
        PlayerPrefs.SetInt("Checkpoint", 1);
        _animator.SetTrigger("LevelChange");
    }
    
    public void NextLevel()
    {
        ChangeLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadLevel()
    {
        GameData.InCheckpoint = true;
        _animator.SetTrigger("LevelChange");
    }

    public void ReloadLevelOnDeath()
    {
        GameData.InCheckpoint = true;
        _healthBar = GameObject.FindWithTag("HealthBar");
        if(_healthBar != null)
            _healthBar.SetActive(false);
        _staminaBar = GameObject.FindWithTag("StaminaBar");
        if(_staminaBar != null)    
            _staminaBar.SetActive(false);
        _animator.SetTrigger("PlayerDead");
    }

    public void OnFadeEnd()
    {
        if (_healthBar != null)
        {
            _healthBar.SetActive(true);
            _healthBar = null;
        }

        if (_staminaBar != null)
        {
            _staminaBar.SetActive(true);
            _staminaBar = null;
        }

        SceneManager.LoadScene(_nextSceneIndex);
    }
}
