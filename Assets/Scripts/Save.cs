

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;



[System.Serializable]
public class Save
{
    public List<string> pressurePlatesActivated;
    public List<string> pickupsPicked;
    public InventoryData InventoryData;
    
    public int checkpointNumber;
    public int levelNumber;

    public Save()
    {
        levelNumber = GameData.LevelNumber;
        checkpointNumber = GameData.CheckpointNumber;
        pressurePlatesActivated = GameData.PressurePlatesActivated;
        pickupsPicked = GameData.PickupsPicked;
        InventoryData = GameData.InventoryData;
    }
}