using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;
    private Enemy _enemy;
    private Attacker _attacker;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _attacker = GetComponent<Attacker>();
        _enemy = GetComponent<Enemy>();
    }

    public void Hit(int damage)
    {
        _damageable.ChangeHealth(-damage);
        _animator.SetTrigger("Stagger");
        
        if(_enemy != null){
            _enemy.setFOV(720); //it's not the player
        }
    }
    
    
}
