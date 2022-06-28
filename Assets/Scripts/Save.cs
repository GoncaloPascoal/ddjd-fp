

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Save
{
    //private List<Item> _pickedUpItems;
    public List<string> pressurePlatesActivated;
    public int checkpointNumber;
    public int levelNumber;
    //private Inventory _inventory;

    public Save()
    {
        levelNumber = GameData.LevelNumber;
        checkpointNumber = GameData.CheckpointNumber;
        pressurePlatesActivated = GameData.PressurePlatesActivated;
    }
}