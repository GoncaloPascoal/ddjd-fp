using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staggerable : MonoBehaviour
{
    private Attacker _attacker;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _attacker = GetComponent<Attacker>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isStaggered()
    {
        Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).IsName("Stagger"));
        return _animator.GetCurrentAnimatorStateInfo(0).IsName("Stagger");
    }
    
    public void Stagger()
    {
        _animator.SetTrigger("Stagger");
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
