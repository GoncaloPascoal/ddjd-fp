using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    private void Awake()
    {
        Enemy.OnEnemyCreated += SetupEnemyBar;
        // Enemy.OnEnemyCreated += enemy => enemy.SetupHealthBar(GetComponentInChildren<Canvas>());
    }

    private void SetupEnemyBar(Enemy enemy)
    {
        enemy.SetupHealthBar(GetComponentInChildren<Canvas>());
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyCreated -= SetupEnemyBar;
    }
}
