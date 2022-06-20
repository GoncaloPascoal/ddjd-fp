
using System;
using System.Collections;
using UnityEngine;

public class DamageablePlayer : Damageable
{
    private Stats _playerStats;

    private void Awake()
    {
        _playerStats = GetComponent<Stats>();
    }

    protected override void Start()
    {
        healthBar = HUD.Instance.healthBar;
        base.Start();
        StartCoroutine(UpdateMaxHealth());
    }

    private IEnumerator UpdateMaxHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            MaxHealth = (int) _playerStats.GetStatValue(StatName.Health);
        }
    }

    protected override void Die()
    {
        
    }
}