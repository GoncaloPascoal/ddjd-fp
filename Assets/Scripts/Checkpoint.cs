using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber = 1;

    private bool _playerInRange;
    private ThirdPersonController _player;
    private GameSaveManager _saveManager;

    private LevelChanger _levelChanger;

    public bool proceedLevel = false;
    
    private bool _activating = false;
    
    private void Start()
    {
        _levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        _saveManager = GameObject.FindWithTag("GameSave").GetComponent<GameSaveManager>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (!_playerInRange) return;

        if (InputManager.Action("Cancel").WasPressedThisFrame())
        {
            if (_player.IsInCheckpoint(checkpointNumber))
            {
                _player.ExitCheckpoint();
            }
        }

        else if (InputManager.Action("Interact").WasPressedThisFrame())
        {
            if (!_player.IsInCheckpoint() && !_activating)
            {
                _activating = true;
                GameData.CheckpointNumber = checkpointNumber;
                //TODO CHANGE THIS
                _saveManager.CreateGameSaveFile();
                _player.EnterCheckpoint(checkpointNumber);
                _levelChanger.ReloadLevel();
            } 
            else if (proceedLevel && _player.IsInCheckpoint(checkpointNumber) && !_activating)
            {
                _levelChanger.NextLevel();
                _saveManager.CreateGameSaveFile();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) _playerInRange = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_playerInRange && other.CompareTag("Player")) _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _playerInRange = false;
    }

    // TODO: change this
    private void OnGUI()
    {
        if (!_playerInRange || _activating) return;
        
        if (!_player.IsInCheckpoint(checkpointNumber))
            GUI.Box(new Rect(140,Screen.height-50,Screen.width-300,120),("Press E to pray"));
        else if (_player.IsInCheckpoint(checkpointNumber))
        {
            GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), (
                "Press Esc to stand up" + (proceedLevel ? "\nPress E to proceed to next level" : "")));
        }
    }
}
