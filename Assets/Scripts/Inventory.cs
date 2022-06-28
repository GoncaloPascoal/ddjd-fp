
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Consumable, uint> Consumables { get; } = new Dictionary<Consumable, uint>();
    public Dictionary<EquipmentSlot, List<Equipment>> Equipment { get; } = new Dictionary<EquipmentSlot, List<Equipment>>();
    public Dictionary<EquipmentSlot, Equipment> Equipped { get; } = new Dictionary<EquipmentSlot, Equipment>();

    [SerializeField] 
    private List<ScriptableObject> itemsTypes = new List<ScriptableObject>();

    private void Awake()
    {
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            Equipment[slot] = new List<Equipment>();
            Equipped[slot] = null;
        }
    }

    private void Start()
    {
        var inventoryData = GameData.InventoryData;
        
        foreach (var consumable in inventoryData.Consumables)
        {
            Debug.Log(consumable.Key);
            var consumableScriptObj = 
                Instantiate(itemsTypes.Find(t => t.name == consumable.Key)) as Consumable;
            
            if (consumableScriptObj != null)
                Consumables.Add(consumableScriptObj, consumable.Value);            
        }
        
        foreach (var equipment in inventoryData.Equipment)
        {
            var piecesList = new List<Equipment>();
            foreach (var equipmentPiece in equipment.Value)
            {
                var equipmentScriptObj = 
                    Instantiate(itemsTypes.Find(t => t.name == equipmentPiece)) as Equipment;
                if (equipmentScriptObj != null)
                    piecesList.Add(equipmentScriptObj);
            }
            Equipment[equipment.Key] = piecesList;
        }

        foreach (var equipped in inventoryData.Equipped)
        {
            var equippedPiece = 
                Instantiate(itemsTypes.Find(t => t.name == equipped.Value)) as Equipment;
            if (equippedPiece != null)
                Equipped[equipped.Key] = equippedPiece;
        }
    }

    public void AddItem(Item item, uint quantity = 1)
    {
        switch (item)
        {
            case Consumable consumable when Consumables.ContainsKey(consumable):
                Consumables[consumable] += quantity;
                break;
            case Consumable consumable:
                Consumables[consumable] = quantity;
                break;
            case Equipment equipment:
                for (uint _ = 0; _ < quantity; ++_)
                    Equipment[equipment.slot].Add(equipment);
                break;
        }
    }

    public void Equip(Equipment equipment)
    {
        Equipped[equipment.slot] = equipment;
        equipment.Equip();
    }

    public void Unequip(EquipmentSlot slot)
    {
        Equipment equipment = Equipped[slot];
        Equipped[slot] = null;
        equipment.Unequip();
    }

    public void Use(Consumable consumable)
    {
        consumable.Use();
        Consumables[consumable] -= 1;
        if (Consumables[consumable] == 0) Consumables.Remove(consumable);
    }

    public float GetEquipmentStatBonus(StatName stat)
    {
        return Equipped.Values.Where(equipment => equipment != null).Sum(equipment => equipment.GetStatValue(stat));
    }
}
