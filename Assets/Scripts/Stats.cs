
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
}
