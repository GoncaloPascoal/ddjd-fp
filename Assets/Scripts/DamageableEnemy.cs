
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
        var enemy = GetComponent<Enemy>();
        if (enemy.backstabbed)
        {
            RestoreToMaxHealth();
            enemy.MindControl();
        }
        else
        {
            foreach (var comp in GetComponents(typeof(Component)))
            {
                if (comp != _animator && comp != transform && comp != this)
                {
                    Destroy(comp);
                }
            }

            // make the ragdoll rigidbodies not kinematic
            foreach (var rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }

            _animator.applyRootMotion = true;
            _animator.SetTrigger("Die");
            gameObject.tag = "Dead";
        }
    }

    public void EndDeath()
    {
        Destroy(_animator);
        Destroy(healthBar.gameObject);
        Destroy(this);
    }
}