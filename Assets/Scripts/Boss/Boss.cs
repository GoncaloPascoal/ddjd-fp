using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private Damageable _damageable;
    
    // Start is called before the first frame update
    void Start()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.InitializeMaxHealth(_maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
