

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    //private List<Item> _pickedUpItems;
    private List<string> _pressurePlatesActivated;
    private int _checkpointNumber;
    //private Inventory _inventory;

    public Save(GameSaveManager gameSaveManager)
    {
        _checkpointNumber = gameSaveManager._checkpointNumber;

        foreach(var plate in gameSaveManager._pressurePlatesActivated)
        {
            _pressurePlatesActivated.Add(GameData.GameObjectToHash(plate.gameObject));
            
        }
        
    }
}