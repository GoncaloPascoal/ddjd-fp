using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField] protected TMP_Text statName, statValue;
    [SerializeField] public StatName stat;

    private static readonly Dictionary<StatName, string> StatRepresentation = new Dictionary<StatName, string>()
    {
        {StatName.Armor, "Armor"},
        {StatName.Damage, "Damage"},
        {StatName.Health, "Maximum Health"},
        {StatName.Stability, "Stability"},
        {StatName.Stamina, "Stamina"},
        {StatName.StaminaRecovery, "Stamina Recovery"},
    };

    private void Start()
    {
        statName.text = StatRepresentation[stat];
    }

    public void SetValue(float value)
    {
        statValue.text = value.ToString("F1");
    }
}
