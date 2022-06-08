using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCanvas : MonoBehaviour
{
    private void Awake()
    {
        Enemy.OnEnemyCreated += enemy => enemy.SetupHealthBar(GetComponentInChildren<Canvas>());
    }
}
