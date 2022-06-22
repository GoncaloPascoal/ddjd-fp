using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staggerable : MonoBehaviour
{
    [SerializeField] private float stabilityRegeneration = 10f;

    private Attacker _attacker;
    private Animator _animator;
    private Stats _stats;

    private float _stability;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();
        _stats = GetComponent<Stats>();

        _stability = MaxStability;
    }

    private float MaxStability => _stats.GetStatValue(StatName.Stability);

    private void Update()
    {
        _stability = Mathf.Min(MaxStability, _stability + stabilityRegeneration * Time.deltaTime);
    }

    public bool IsStaggered()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Stagger");
    }

    public bool Stagger(float damage)
    {
        _stability -= damage;

        if (_stability > 0) return false;

        _animator.SetTrigger("Stagger");
        _stability = MaxStability;
        return true;
    }

    public void StopStagger()
    {
        _animator.SetBool("WillNotStagger",true);
    }

    public void ActivateStaggerNormalAttack()
    {
        _animator.SetBool("WillNotStagger",false);
        _animator.ResetTrigger("Stagger");
    }

    public void ActivateStagger()
    {
        _attacker.EndAttack();
        _animator.SetBool("WillNotStagger",false);
        _animator.ResetTrigger("Stagger");
    }
}
