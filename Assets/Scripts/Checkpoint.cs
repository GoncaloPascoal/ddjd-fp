using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private int checkpointNumber = 1;
    
    private bool _playerInRange;
    private ThirdPersonController _player;

    private LevelChanger _levelChanger;

    // Start is called before the first frame update
    void Start()
    {
        _levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (_playerInRange && !_player.IsInCheckpoint())
            {
                PlayerPrefs.SetInt("Checkpoint", checkpointNumber);
                _player.EnterCheckpoint(checkpointNumber);
                _levelChanger.ReloadLevel();
            }
            else if (_player.GetCheckpoint() == checkpointNumber)
            {
                _player.ExitCheckpoint();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_playerInRange && other.CompareTag("Player"))
            _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    // TODO: change this 
    private void OnGUI()
    {
        if (_playerInRange && !_player.IsInCheckpoint())
            GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to pray"));
        else if (_player.IsInCheckpoint())
            GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to stand up"));
        
    }
}
