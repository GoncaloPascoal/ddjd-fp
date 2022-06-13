using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;
    private Enemy _enemy;

    private void Start()
    {
        _damageable = GetComponent<Damageable>();
        _enemy = GetComponent<Enemy>();
    }

    public void Hit(int damage)
    {
        _damageable.ChangeHealth(-damage);
        if(_enemy != null){
            _enemy.setFOV(720); //it's not the player
        }
    }
}
