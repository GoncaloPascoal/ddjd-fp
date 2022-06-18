using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    protected Animator _animator;
    
    [SerializeField]
    private List<string> attackingStates;
    
    [SerializeField]
    protected Weapon weapon;

    private bool _bufferedAttack = false;

    protected bool _isAttacking = false;
    private bool _isStartingAttacking = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAttack()
    {
        // Debug.Log("start attack");
        _animator.SetBool("AttackNormal", false);
        _isStartingAttacking = true;
    }

    public void AttackMoment()
    {
        _isStartingAttacking = false;
    }

    public void EndAttack()
    {
        // Debug.Log("EndAttack called");
        if (_bufferedAttack)
        {
            weapon.Attack();
            _bufferedAttack = false;
            _isAttacking = true;
            // _animator.SetTrigger("AttackNormal");
        }
        else
        {
            // Debug.Log("ENDING");
            _isAttacking = false;
            weapon.DisableColider();
            _animator.applyRootMotion = false;
        }
    }

    public void Attack()
    {
        //Debug.Log("Normal Attack");

        var animatorState = _animator.GetCurrentAnimatorStateInfo(0);
        
        // if already attacking, buffer next attack if the attack animation if at least half-way through
        if (IsAttacking())
        {
            if (inAttackingState(animatorState) && animatorState.normalizedTime > 0.2f)
            {
                _bufferedAttack = true;
                _animator.SetBool("AttackNormal", true);
                _animator.applyRootMotion = true;
            }
        }
        else
        {
            _isAttacking = true;
            weapon.Attack();
            _animator.SetBool("AttackNormal", true);
            _animator.applyRootMotion = true;
        }
    }

    //Plays an attack animation without having to make any animation buffer
    public void AttackNotBuffered(List<string> possibleAnimations)
    {
        if (IsAttacking())
            return;
        
        
        int randomAnimation = Random.Range(0, possibleAnimations.Count);

        _isAttacking = true;
        weapon.Attack();
        _animator.SetTrigger(possibleAnimations[randomAnimation]);
    }

    public bool IsAttacking()
    {
        return _isAttacking;
    }

    public bool IsStartingAttack()
    {
        if (_isAttacking)
        {
            return _isStartingAttacking;
        }

        return false;
    }

    public void ResetAlreadyHit()
    {
        weapon.ResetAlreadyHit();
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
