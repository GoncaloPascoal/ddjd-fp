
using System;

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
public class Stats
{
    public SerializableDictionary<StatName, float> baseValues;
}
