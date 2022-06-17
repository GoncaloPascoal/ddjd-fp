
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
public class StatsDictionary : SerializableDictionary<StatName, float> { }

[Serializable]
public class Stats
{
    public StatsDictionary values;
}
