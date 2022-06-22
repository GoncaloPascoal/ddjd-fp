
using System;
using UnityEngine;

public enum StatName
{
    Health,
    Stamina,
    StaminaRecovery,
    Damage,
    Armor,
    Stability
}

[Serializable]
public class StatsDictionary : SerializableDictionary<StatName, float> { }

public class Stats : MonoBehaviour
{
    public StatsDictionary baseValues;

    public virtual float GetStatValue(StatName stat)
    {
        return baseValues[stat];
    }

    public static float CalculateReducedDamage(float damage, float armor)
    {
        return damage - Mathf.Min(damage * 0.75f, Mathf.Pow(armor, 0.93f));
    }
}
