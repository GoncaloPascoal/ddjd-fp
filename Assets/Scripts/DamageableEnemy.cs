
using System;
using UnityEngine;

public class DamageableEnemy : Damageable
{
    private Animator _animator;
    private GameObject _souls;
    private Attacker _attacker;
    private Hittable _hittable;
    private BoxCollider _backstab;
    private bool _alreadyDied;

    [SerializeField]
    private int experienceGiven = 1;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();
        _hittable = GetComponent<Hittable>();
        _backstab = gameObject.transform.Find("Backstab").gameObject.GetComponent<BoxCollider>();
        _souls = gameObject.transform.Find("FloatingSoul").gameObject;
        _alreadyDied = false;
    }

    public void DeleteComponents()
    {
        foreach (Component comp in GetComponents(typeof(Component)))
        {
            if (comp != _animator && comp != transform && comp != this)
            {
                Destroy(comp);
            }
        }

        // Make ragdoll rigidbodies not kinematic
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
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
    
    public override void Die()
    {
        Enemy enemy = GetComponent<Enemy>();
        if (_attacker != null) _attacker.DisableWeapon();

        if (enemy.backstabbed && !enemy.CompareTag("MindControlled"))
        {
            RestoreToMaxHealth();
            enemy.MindControl();
        }
        else
        {
            if (_alreadyDied)
            {
                DeleteComponents();
            }
            else
            {
                foreach (Component comp in GetComponents(typeof(CapsuleCollider)))
                {
                    ((CapsuleCollider) comp).enabled = false;
                }
            }

            _animator.applyRootMotion = true;
            _animator.SetTrigger("Die");
            gameObject.tag = "Dead";

            if (!enemy.CompareTag("MindControlled")) LevelSystem.Instance.AddExperience(experienceGiven);
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
            foreach (Component comp in GetComponents(typeof(CapsuleCollider)))
            {
                ((CapsuleCollider) comp).enabled = false;
            }

            enabled = false;
            return;
        }

        DeleteAnimator();
    }
}