using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private Damageable _damageable;

    private Animator _animator;
    private Attacker _attacker;

    public float defaultAttackCooldown = 2f;
    
    private float _attackCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.InitializeMaxHealth(_maxHealth);

        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();

        _attackCooldown = defaultAttackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        _attacker.AttackNotBuffered(new List<string> {"Multiple Slashes"});
    }

    public void LookAtTarget(Vector3 target)
    {
        Quaternion lookRotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
            Quaternion.LookRotation(target - transform.position).eulerAngles.y,
            transform.rotation.eulerAngles.z);
        transform.rotation =
            Quaternion.Lerp(transform.rotation, lookRotation,
                Time.deltaTime * 5f);
    }

    public float GetAttackCooldown()
    {
        return _attackCooldown;
    }

    public float ReduceAttackCooldown()
    {
        _attackCooldown -= Time.deltaTime;
        return _attackCooldown;
    }
    
    public void SetAttackCooldown(float cooldown)
    {
        _attackCooldown = cooldown;
    }
}
