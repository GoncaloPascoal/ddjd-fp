using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;
    private Enemy _enemy;
    private Animator _animator;
    private Staggerable _staggerable;
    private EntitySounds _entitySounds;

    [SerializeField] private int hitSoundChance = 50;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _enemy = GetComponent<Enemy>();
        _staggerable = GetComponent<Staggerable>();
        _entitySounds = GetComponent<EntitySounds>();
    }

    public void Hit(int damage)
    {
        _damageable.ChangeHealth(-damage);

        var isStaggered = false;
        
        if (_staggerable != null && _staggerable.enabled)
        {
            isStaggered = _staggerable.Stagger();
        }

        if (_entitySounds != null)
            _entitySounds.GetHitSound(isStaggered ? 100 : hitSoundChance);
        
        if (_enemy != null) {
            _enemy.SetFOV(720); //it's not the player
        }
    }
}
