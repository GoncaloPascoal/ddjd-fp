using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StarterAssets;
using UnityEngine;
using Random = UnityEngine.Random;

public class Attacker : MonoBehaviour
{
    [SerializeField] private List<string> attackingStates;
    [SerializeField] protected Weapon weapon;

    protected Animator _animator;
    protected ThirdPersonController _tpc;

    private bool _bufferedAttack = false;

    protected bool _isAttacking = false;
    private bool _isStartingAttack = false;

    private string _currentTrigger;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _tpc = GetComponent<ThirdPersonController>();
    }

    public void StartAttack()
    {
        if(_tpc!=null && _currentTrigger != null) _tpc.ChangeStamina(_tpc.StaminaUsageAttacks[_currentTrigger]);
        _isStartingAttack = true;
    }

    public void ResetAlreadyHit()
    {
        weapon.ResetAlreadyHit();
    }

    public void AttackMoment()
    {
        weapon.Attack();
        _isStartingAttack = false;
    }

    public void EndAttack()
    {
        if (_bufferedAttack)
        {
            _bufferedAttack = false;
            _isAttacking = true;
        }
        else
        {
            _currentTrigger = null;
            _bufferedAttack = false;
            _isAttacking = false;
            weapon.DisableCollider();
            _animator.applyRootMotion = false;
        }
    }

    public void Attack(string triggerName)
    {
        AnimatorStateInfo animatorState = _animator.GetCurrentAnimatorStateInfo(0);

        // If already attacking, buffer next attack
        if (IsAttacking())
        {
            if (InAttackingState(animatorState) && animatorState.normalizedTime > 0.3f)
            {
                _bufferedAttack = true;
                if (_currentTrigger != null) _animator.ResetTrigger(_currentTrigger);
                _animator.SetTrigger(_currentTrigger);
                _currentTrigger = triggerName;
                _animator.applyRootMotion = true;
            }
        }
        else
        {
            _isAttacking = true;
            _animator.SetTrigger(triggerName);
            _currentTrigger = triggerName;
            _animator.applyRootMotion = true;
            // _tpc.ChangeStamina(_tpc.StaminaUsageAttacks[triggerName]);
        }
    }

    // Plays an attack animation without having to make any animation buffer
    public void AttackNotBuffered(List<string> possibleAnimations)
    {
        if (IsAttacking()) return;

        int randomAnimation = Random.Range(0, possibleAnimations.Count);

        _isAttacking = true;
        _animator.applyRootMotion = true;
        weapon.Attack();
        _animator.SetTrigger(possibleAnimations[randomAnimation]);
    }

    public void AttackNotBuffered()
    {
        AttackNotBuffered(attackingStates);
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
