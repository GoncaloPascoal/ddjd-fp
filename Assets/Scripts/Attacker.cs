using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private List<string> attackingStates;
    [SerializeField] private Weapon weapon;

    private Animator _animator;

    private bool _bufferedAttack = false;

    private bool _isAttacking = false;
    private bool _isStartingAttacking = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        _animator.SetBool("AttackNormal", false);
        _isStartingAttacking = true;
    }

    public void AttackMoment()
    {
        _isStartingAttacking = false;
    }

    public void EndAttack()
    {
        if (_bufferedAttack)
        {
            weapon.Attack();
            _bufferedAttack = false;
            _isAttacking = true;
            // _animator.SetTrigger("AttackNormal");
        }
        else
        {
            _isAttacking = false;
            weapon.DisableCollider();
            _animator.applyRootMotion = false;
        }
    }

    public void Attack()
    {
        AnimatorStateInfo animatorState = _animator.GetCurrentAnimatorStateInfo(0);

        // if already attacking, buffer next attack if the attack animation if at least half-way through
        if (IsAttacking())
        {
            if (InAttackingState(animatorState) && animatorState.normalizedTime > 0.2f)
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
        return _isAttacking && _isStartingAttacking;
    }

    private bool InAttackingState(AnimatorStateInfo stateInfo)
    {
        return attackingStates.Any(stateInfo.IsName);
    }
}
