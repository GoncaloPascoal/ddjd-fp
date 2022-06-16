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
        Debug.Log("Cant be staggered anymore");
        _animator.SetBool("WillNotStagger",true);
    }

    public void ActivateStaggerNormalAttack()
    {
        Debug.Log("ActivateStaggerNormalAttack");
        _animator.SetBool("WillNotStagger",false);
    }
    
    public void ActivateStagger()
    {
        Debug.Log("ActivateStagger");
        _attacker.EndAttack();
        _animator.SetBool("WillNotStagger",false);
    }
}
