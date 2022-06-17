
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "FlatHealingItem", menuName = "ScriptableObject/Item/Consumable/FlatHealingItem")]
public class FlatHealingItem : Consumable
{
    public int healing;
    [NonSerialized] private Damageable _damageable;

    private void OnEnable()
    {
        _damageable = GameObject.FindWithTag("Player").GetComponent<Damageable>();
    }

    public override void Use()
    {
        _damageable.ChangeHealth(healing);
    }
}