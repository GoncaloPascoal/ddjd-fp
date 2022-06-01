using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    private Animator _animator;
    
    [SerializeField]
    private List<string> attackingStates;
    
    [SerializeField]
    private Weapon weapon;

    private bool _bufferedAttack = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndAttack()
    {
        if (_bufferedAttack)
        {
            weapon.Attack();
            _bufferedAttack = false;
        }
    }

    public void Attack()
    {
        //Debug.Log("Normal Attack");

        // if already attacking, buffer next attack if the attack animation if at least half-way through
        if (IsAttacking() && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
            _bufferedAttack = true;
        else
            weapon.Attack();
        
        _animator.SetTrigger("AttackNormal");
    }
    
    public bool IsAttacking()
    {
        foreach (var state in attackingStates)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(state))
                return true;
        }

        return false;
    }
}
