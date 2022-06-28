
using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class DamageablePlayer : Damageable
{
    private Stats _playerStats;
    private LevelChanger _levelChanger;
    
    private void Awake()
    {
        _playerStats = GetComponent<Stats>();
    }

    protected override void Start()
    {
        _levelChanger = GameObject.Find("LevelChanger").GetComponent<LevelChanger>();
        healthBar = HUD.Instance.healthBar;
        base.Start();
        StartCoroutine(UpdateMaxHealth());
    }

    private IEnumerator UpdateMaxHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            MaxHealth = _playerStats.GetStatValue(StatName.Health);
        }
    }

    protected override void Die()
    {
        _levelChanger.ReloadLevel();   
    }
}