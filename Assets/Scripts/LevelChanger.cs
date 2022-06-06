using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private Animator _animator;

    private int _nextSceneIndex;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Checkpoint"))
        {
            PlayerPrefs.SetInt("Checkpoint", 1); // TODO: move this to main menu script?
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _nextSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void OnFadeEnd()
    {
        SceneManager.LoadScene(_nextSceneIndex);
    }
}
