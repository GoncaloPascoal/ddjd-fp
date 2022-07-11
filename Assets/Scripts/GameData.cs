using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(StatsDictionary))]
[CustomPropertyDrawer(typeof(ItemPickupDictionary))]
[CustomPropertyDrawer(typeof(MenuButtonStateSpriteDictionary))]
[CustomPropertyDrawer(typeof(MenuTabGameObjectDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }
#endif

[Serializable]
public struct InventoryData
{
    public SerializableDictionary<string, uint> Consumables;
    public SerializableDictionary<EquipmentSlot, List<string>> Equipment;
    public SerializableDictionary<EquipmentSlot, string> Equipped;
}

[Serializable]
public struct LevelSystemData
{
    public int Experience;
    public int Level;
    public SerializableDictionary<StatName, int> StatPoints;
}


public static class GameData
{
    private static List<string> _pickupsPicked = new List<string>();
    public static List<string> PickupsPicked
    {
        get => _pickupsPicked;
        set => _pickupsPicked = value;
    }
    
    private static List<string> _pressurePlatesActivated = new List<string>();
    public static List<string> PressurePlatesActivated
    {
        get => _pressurePlatesActivated;
        set => _pressurePlatesActivated = value;
    }

    public static int LevelNumber = 1;
    public static int CheckpointNumber = 1;
    private static InventoryData _inventoryData = new InventoryData()
    {
        Consumables = new SerializableDictionary<string, uint>(),
        Equipment = new SerializableDictionary<EquipmentSlot, List<string>>(),
        Equipped = new SerializableDictionary<EquipmentSlot, string>()
    };

    public static InventoryData InventoryData
    {
        get => _inventoryData;
        set => _inventoryData = value;
    }

    private static void GetInventoryData()
    {
        InventoryData = new InventoryData()
        {
            Consumables = new SerializableDictionary<string, uint>(),
            Equipment = new SerializableDictionary<EquipmentSlot, List<string>>(),
            Equipped = new SerializableDictionary<EquipmentSlot, string>()
        };
        var newConsumables = new SerializableDictionary<string, uint>();
        var newEquipment = new SerializableDictionary<EquipmentSlot, List<string>>();
        var newEquipped = new SerializableDictionary<EquipmentSlot, string>();

        var inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
        if (inventory == null) return;

        foreach (var consumable in inventory.Consumables)
        {
            newConsumables.Add(consumable.Key.name, consumable.Value);
        }
        
        foreach (var equipment in inventory.Equipment)
        {
            var piecesList = new List<string>();
            foreach (var equipmentPiece in equipment.Value)
            {
                piecesList.Add(equipmentPiece.name);
            }
            newEquipment.Add(equipment.Key, piecesList);
        }

        foreach (var equipped in inventory.Equipped)
        {
            if (equipped.Value != null)
                newEquipped.Add(equipped.Key, equipped.Value.name);
        }
        
        InventoryData = new InventoryData()
        {
            Consumables = newConsumables,
            Equipment = newEquipment,
            Equipped = newEquipped
        };

    }
    
    private static LevelSystemData _levelSystemData = new LevelSystemData()
    {
        Experience = 0,
        Level = 1,
        StatPoints = new SerializableDictionary<StatName, int>(LevelSystem.InitializeStatPoints())
    };

    public static LevelSystemData LevelSystemData
    {
        get => _levelSystemData;
        set => _levelSystemData = value;
    }

    private static void GetLevelSystemData()
    {
        var levelSystem = GameObject.FindWithTag("Player").GetComponent<LevelSystem>();
        if (levelSystem == null) return;
        
        LevelSystemData = new LevelSystemData()
        {
            Experience = levelSystem.Experience,
            Level = levelSystem.Level,
            StatPoints = new SerializableDictionary<StatName, int>(levelSystem.GetCurrentStats())
        };
    }

    public static bool InCheckpoint = false;
    
    public static void AddPickUp(GameObject pickupObject)
    {
        PickupsPicked = new List<string> (PickupsPicked) {GameObjectToHash(pickupObject)};
    }

    public static void AddActivatedPressurePlate(PressurePlate pressurePlate)
    {
        PressurePlatesActivated = new List<string> (PressurePlatesActivated) 
            {GameObjectToHash(pressurePlate.gameObject)};
    }

    public static string GameObjectToHash(GameObject gameObject)
    {
        var hash = new Hash128();
        hash.Append(gameObject.name);
        hash.Append(gameObject.tag);

        var position = gameObject.transform.position;
        hash.Append(position.x);
        hash.Append(position.y);
        hash.Append(position.z);

        var rotation = gameObject.transform.rotation;
        hash.Append(rotation.x);
        hash.Append(rotation.y);
        hash.Append(rotation.z);

        var localScale = gameObject.transform.localScale;
        hash.Append(localScale.x);
        hash.Append(localScale.y);
        hash.Append(localScale.z);

        return hash.ToString();
    }

    public static void SetSaveData(Save save)
    {
        LevelNumber = save.levelNumber;
        CheckpointNumber = save.checkpointNumber;
        _pressurePlatesActivated = save.pressurePlatesActivated;
        _pickupsPicked = save.pickupsPicked;
        InventoryData = save.InventoryData;
        LevelSystemData = save.LevelSystemData;
    }

    public static Save GetSaveData()
    {
        GetInventoryData();
        GetLevelSystemData();
        return new Save();
    }

    public static void NewLevel(int levelIndex)
    {
        LevelNumber = levelIndex;
        CheckpointNumber = 1;
    }

    public static Save NewSave()
    {
        LevelNumber = 1;
        CheckpointNumber = 1;
        _pressurePlatesActivated = new List<string>();
        _pickupsPicked = new List<string>();
        InventoryData = new InventoryData()
        {
            Consumables = new SerializableDictionary<string, uint>(),
            Equipment = new SerializableDictionary<EquipmentSlot, List<string>>(),
            Equipped = new SerializableDictionary<EquipmentSlot, string>()
        };
        LevelSystemData = new LevelSystemData()
        {
            Experience = 0,
            Level = 1,
            StatPoints = new SerializableDictionary<StatName, int>(LevelSystem.InitializeStatPoints())
        };

        return new Save();
    }
}
