using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Attacker : MonoBehaviour
{
    [SerializeField] private List<string> attackingStates;
    [SerializeField] protected Weapon weapon;

    protected Animator _animator;

    private bool _bufferedAttack = false;

    protected bool _isAttacking = false;
    private bool _isStartingAttack = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartAttack()
    {
        _isStartingAttack = true;
    }

    public void AttackMoment()
    {
        _isStartingAttack = false;
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

        // If already attacking, buffer next attack if the attack animation if at least half-way through
        if (IsAttacking())
        {
            if (InAttackingState(animatorState) && animatorState.normalizedTime > 0.1f)
            {
                _bufferedAttack = true;
                _animator.SetTrigger("AttackNormal");
                _animator.applyRootMotion = true;
            }
        }
        else
        {
            _isAttacking = true;
            weapon.Attack();
            _animator.SetTrigger("AttackNormal");
            _animator.applyRootMotion = true;
        }
    }

    // Plays an attack animation without having to make any animation buffer
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
        return _isAttacking && _isStartingAttack;
    }

    public void SetTargets(List<string> newTargets)
    {
        weapon.SetTargetTags(newTargets);
    }

    private bool InAttackingState(AnimatorStateInfo stateInfo)
    {
        return attackingStates.Any(stateInfo.IsName);
    }
}
