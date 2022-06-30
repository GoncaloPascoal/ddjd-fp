
using System;
using UnityEngine;

[Serializable]
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
        return damage - Mathf.Min(damage * 0.65f, Mathf.Pow(armor, 0.9f));
    }
}
