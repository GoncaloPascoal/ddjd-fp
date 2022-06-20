
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<Consumable, uint> Consumables { get; } = new Dictionary<Consumable, uint>();
    public Dictionary<EquipmentSlot, List<Equipment>> Equipment { get; } = new Dictionary<EquipmentSlot, List<Equipment>>();
    public Dictionary<EquipmentSlot, Equipment> Equipped { get; } = new Dictionary<EquipmentSlot, Equipment>();

    private void Awake()
    {
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            Equipment[slot] = new List<Equipment>();
            Equipped[slot] = null;
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
