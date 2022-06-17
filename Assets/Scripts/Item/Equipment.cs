
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentSlot
{
    Armor,
    Weapon,
    Ring,
    Necklace,
}

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObject/Item/Equipment")]
public class Equipment : Item
{
    [SerializeField] private StatsDictionary statValues;
    public EquipmentSlot slot;

    public virtual float GetStatValue(StatName stat)
    {
        return statValues.ContainsKey(stat) ? statValues[stat] : 0f;
    }

    public virtual void Equip() { }
    public virtual void Unequip() { }
}
