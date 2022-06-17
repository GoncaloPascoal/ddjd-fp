using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staggerable : MonoBehaviour
{
    private Attacker _attacker;
    private Animator _animator;

    [SerializeField] private float superArmorMax = 100f;
    [SerializeField] private float superArmorReductionPerHit = 30f;
    [SerializeField] private float superArmorRegenPerSecond = 5f;
    private float _currentSuperArmor;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();
        _currentSuperArmor = superArmorMax;
    }

    // Update is called once per frame
    void Update()
    {
        float regen = superArmorRegenPerSecond * Time.deltaTime;
        _currentSuperArmor += regen;
        if (_currentSuperArmor > superArmorMax)
            _currentSuperArmor = superArmorMax;
    }

    public bool isStaggered()
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Stagger");
    }
    
    public void Stagger()
    {
        _currentSuperArmor -= superArmorReductionPerHit;
        Debug.Log(_currentSuperArmor);
        if (_currentSuperArmor <= 0)
        {
            _animator.SetTrigger("Stagger");
            _currentSuperArmor = superArmorMax;
        }
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
