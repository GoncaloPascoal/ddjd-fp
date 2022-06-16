using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;
    private Enemy _enemy;
    private Animator _animator;
    private Staggerable _staggerable;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _enemy = GetComponent<Enemy>();
        _staggerable = GetComponent<Staggerable>();
    }

    public void Hit(int damage)
    {
        _damageable.ChangeHealth(-damage);
        
        if (_staggerable != null && _staggerable.enabled)
            _staggerable.Stagger();
        
        if(_enemy != null){
            _enemy.setFOV(720); //it's not the player
        }
    }
    
    
}
