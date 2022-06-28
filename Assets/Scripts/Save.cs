

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    //private List<Item> _pickedUpItems;
    private List<string> _pressurePlatesActivated;
    private int _checkpointNumber;
    //private Inventory _inventory;

    public Save()
    {
        _checkpointNumber = GameData._checkpointNumber;
        
        foreach(var plate in GameData.PressurePlatesActivated)
        {
            _pressurePlatesActivated.Add(GameData.GameObjectToHash(plate.gameObject));
            
        }
        
    }
}