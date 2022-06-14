
using System;
using UnityEngine;

public class DamageableEnemy : Damageable
{
    private Animator _animator;
    protected GameObject _souls;
    private Attacker _attacker;
    private Hittable _hittable;


    private new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();
        _hittable = GetComponent<Hittable>();
        _souls = gameObject.transform.Find("FloatingSoul").gameObject;
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
            
            foreach (var comp in GetComponents(typeof(CapsuleCollider)))
            {
                ((CapsuleCollider) comp).enabled = false;
            }

            _animator.applyRootMotion = true;
            _animator.SetTrigger("Die");
            gameObject.tag = "Dead";
        }
    }

    public void EndDeath()
    {
        _souls.SetActive(true);
        // _animator.gameObject.SetActive(false);
        healthBar.gameObject.SetActive(false);
        _hittable.enabled = false;
        _attacker.enabled = false;
        foreach (var comp in GetComponents(typeof(CapsuleCollider)))
        {
            ((CapsuleCollider) comp).enabled = false;
        }
        
        
        enabled = false;
    }
}