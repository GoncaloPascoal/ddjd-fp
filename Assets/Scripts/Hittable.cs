using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;
    private Stats _stats;
    private Enemy _enemy;
    private Animator _animator;
    private Staggerable _staggerable;
    private EntitySounds _entitySounds;

    [SerializeField] private int hitSoundChance = 50;

    private void Start()
    {
        _damageable = GetComponent<Damageable>();
        _stats = GetComponent<Stats>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _staggerable = GetComponent<Staggerable>();
        _entitySounds = GetComponent<EntitySounds>();
    }

    public void Hit(float damage)
    {
        float reducedDamage = damage;
        if (_stats != null) reducedDamage = Stats.CalculateReducedDamage(damage, _stats.GetStatValue(StatName.Armor));
        _damageable.ChangeHealth(-reducedDamage);

        bool isStaggered = false;
        if (_staggerable != null && _staggerable.enabled)
        {
            isStaggered = _staggerable.Stagger(damage);
        }

        if (_entitySounds != null) _entitySounds.GetHitSound(isStaggered ? 100 : hitSoundChance);

        if (_enemy != null) {
            _enemy.SetFOV(720); //it's not the player
        }
    }
}
