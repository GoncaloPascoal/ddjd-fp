using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;
    private Enemy _enemy;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _enemy = GetComponent<Enemy>();
    }

    public void Hit(int damage)
    {
        _damageable.ChangeHealth(-damage);
        _animator.SetTrigger("Stagger");
        
        if(_enemy != null){
            _enemy.setFOV(720); //it's not the player
        }
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
        _enemy.GetComponent<Attacker>().EndAttack();
        _animator.SetBool("WillNotStagger",false);
    }
}
