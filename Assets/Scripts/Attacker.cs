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

    private bool isAttacking = false;
    
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
            isAttacking = true;
            _animator.SetTrigger("AttackNormal");
        }
        else
        {
            isAttacking = false;
            weapon.EndAttack();
        }
    }

    public void Attack()
    {
        //Debug.Log("Normal Attack");

        var animatorState = _animator.GetCurrentAnimatorStateInfo(0);
        
        // if already attacking, buffer next attack if the attack animation if at least half-way through
        if (IsAttacking())
        {
            if (inAttackingState(animatorState) && animatorState.normalizedTime > 0.5f)
                _bufferedAttack = true;
        }
        else
        {
            isAttacking = true;
            weapon.Attack();
            _animator.SetTrigger("AttackNormal");

        }
    }

    //Plays an attack animation without having to make any animation buffer
    public void AttackNotBuffered(List<string> possibleAnimations)
    {
        if (IsAttacking())
            return;
        
        int randomAnimation = Random.Range(0, possibleAnimations.Count);

        isAttacking = true;
        weapon.Attack();
        _animator.SetTrigger(possibleAnimations[randomAnimation]);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    private bool inAttackingState(AnimatorStateInfo stateInfo)
    {
        foreach (var state in attackingStates)
        {
            if (stateInfo.IsName(state))
                return true;
        }
        return false;
    }
}
