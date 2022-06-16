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
    
    public void StopStagger()
    {
        _animator.SetBool("WillNotStagger",true);
    }

    public void ActivateStaggerNormalAttack()
    {
        _animator.SetBool("WillNotStagger",false);
    }
    
    public void ActivateStagger()
    {
        _attacker.EndAttack();
        _animator.SetBool("WillNotStagger",false);
    }
}
