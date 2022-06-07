
using System;
using UnityEngine;

public class DamageableEnemy : Damageable
{
    private Animator _animator;

    private new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    protected override void Die()
    {
        foreach (var comp in GetComponents(typeof(Component)))
        {
            if (comp != _animator && comp != transform && comp != this)
            {
                Destroy(comp);
            }
        }

        _animator.applyRootMotion = true;
        _animator.SetTrigger("Die");
    }

    public void EndDeath()
    {
        Destroy(_animator);
        Destroy(healthBar.gameObject);
        Destroy(this);
    }
}