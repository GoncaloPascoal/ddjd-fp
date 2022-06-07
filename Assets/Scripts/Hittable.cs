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
}
