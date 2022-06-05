using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private Animator _animator;

    private int _nextSceneIndex;
    
    
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
        _animator.SetTrigger("LevelChange");
    }
    
    public void NextLevel()
    {
        _nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        _animator.SetTrigger("LevelChange");
    }

    public void ReloadLevel()
    {
        _animator.SetTrigger("LevelChange");
    }

    public void OnFadeEnd()
    {
        SceneManager.LoadScene(_nextSceneIndex);
    }
}
