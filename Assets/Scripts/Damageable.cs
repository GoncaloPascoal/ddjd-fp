using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private int _maxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            OnMaxHealthChanged?.Invoke();
        }
    }

    private int _health;
    public int Health
    {
        get => _health;
        set
        {
            _health = value;
            OnHealthChanged?.Invoke();
        }
    }
    
    public Action OnMaxHealthChanged, OnHealthChanged;

    public void ChangeHealth(int delta)
    {
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
    }

    public void InitializeMaxHealth(int value)
    {
        MaxHealth = value;
        Health = value;
    }
}
