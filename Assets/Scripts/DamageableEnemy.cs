
using System;
using UnityEngine;

public class DamageableEnemy : Damageable
{
    private Animator _animator;
    protected GameObject _souls;
    private Attacker _attacker;
    private Hittable _hittable;
    private BoxCollider _backstab;
    private bool _alreadyDied;



    private new void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();
        _hittable = GetComponent<Hittable>();
        _backstab = gameObject.transform.Find("Backstab").gameObject.GetComponent<BoxCollider>();
        _souls = gameObject.transform.Find("FloatingSoul").gameObject;
        _alreadyDied = false;
    }

    public void DeleteComps()
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
    }

    public void DeleteAnimator()
    {
        Destroy(_animator);
        Destroy(healthBar.gameObject);
        Destroy(this);
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
            if (_alreadyDied)
            {
                DeleteComps();
            }
            else
            {
                foreach (var comp in GetComponents(typeof(CapsuleCollider)))
                {
                    ((CapsuleCollider) comp).enabled = false;
                }   
            }
            

            _animator.applyRootMotion = true;
            _animator.SetTrigger("Die");
            gameObject.tag = "Dead";
        }
    }

    public void EndDeath()
    {
        if (!_alreadyDied)
        {
            _souls.SetActive(true);
            _alreadyDied = true;

            // _animator.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
            _hittable.enabled = false;
            if(_attacker != null) _attacker.enabled = false; // ranged enemy does not have the attacker script
            _backstab.enabled = false;
            foreach (var comp in GetComponents(typeof(CapsuleCollider)))
            {
                ((CapsuleCollider) comp).enabled = false;
            }


            enabled = false;
            return;
        }
        
        DeleteAnimator();
    }
}