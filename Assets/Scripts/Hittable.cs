using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hittable : MonoBehaviour
{
    private Damageable _damageable;

    private void Start()
    {
        _damageable = GetComponent<Damageable>();
    }

    public void Hit(int damage)
    {
        _damageable.ChangeHealth(-damage);
        Debug.Log("Ouch! Current HP: " + _damageable.Health + ".");
    }

    public void GetHitBackstab(int damage)
    {
        //if (curHp <= 0)
        //{
        //    _enemy.mindControl();
        //}
        //_animator.SetBool("Backstab", true);
    }

    public void FreezeForBackstab()
    {
        //_isBackstabbing = true;
        //_enemy.SetBackstabbing(true);
    }

    public void EndBackstab()
    {
        //_animator.SetBool("Backstab", false);
        //_isBackstabbing = false;
        //_enemy.SetBackstabbing(false);
    }
}
