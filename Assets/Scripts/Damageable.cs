using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    private int _maxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        private set
        {
            _maxHealth = value;
            OnMaxHealthChanged?.Invoke();
        }
    }

    private int _health;
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            OnHealthChanged?.Invoke();
        }
    }

    public Action OnMaxHealthChanged, OnHealthChanged;

    public void ChangeHealth(int delta)
    {
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
        if (Health == 0) Die();
    }

    public void InitializeMaxHealth(int value)
    {
        MaxHealth = value;
        Health = value;
    }

    protected abstract void Die();
}
