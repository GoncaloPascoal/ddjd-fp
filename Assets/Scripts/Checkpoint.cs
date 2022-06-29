using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static readonly string[] PromptButtonsInteract = { "E", "(A)" },
        PromptButtonsCancel = { "Escape", "(B)" };
    private const string PromptActionSave = "Pray",
        PromptActionLeave = "Stand Up",
        PromptActionContinue = "Proceed to Next Level";

    public int checkpointNumber = 1;

    private bool _playerInRange;
    private ThirdPersonController _player;
    private GameSaveManager _saveManager;

    private LevelChanger _levelChanger;

    public bool proceedLevel = false;
    
    private bool _activating = false;
    private bool _promptShown;

    private void Start()
    {
        _levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        _saveManager = GameObject.FindWithTag("GameSave").GetComponent<GameSaveManager>();
        _player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
        _promptShown = false;
    }

    private void Update()
    {
        if (!_playerInRange) return;

        if (InputManager.Action("Cancel").WasPressedThisFrame())
        {
            if (_player.IsInCheckpoint(checkpointNumber))
            {
                _player.ExitCheckpoint();
                if (_promptShown)
                {
                    HUD.Instance.HideButtonPrompt();
                    _promptShown = false;
                }
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
        if (other.CompareTag("Player"))
        {
            if (!_promptShown)
            {
                if (!_player.IsInCheckpoint(checkpointNumber))
                {
                    HUD.Instance.ShowButtonPrompt(PromptButtonsInteract, PromptActionSave);
                }
                else if (proceedLevel)
                {
                    HUD.Instance.ShowButtonPromptRaw($"{string.Join(", ", PromptButtonsInteract)}: " +
                                                     $"{PromptActionContinue}\n{string.Join(", ", PromptButtonsCancel)}: {PromptActionLeave}");
                }
                else
                {
                    HUD.Instance.ShowButtonPrompt(PromptButtonsCancel, PromptActionLeave);
                }

                _promptShown = true;
            }
            _playerInRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_playerInRange && other.CompareTag("Player")) _playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_promptShown)
            {
                HUD.Instance.HideButtonPrompt();
                _promptShown = false;
            }
            _playerInRange = false;
        }
    }
}
