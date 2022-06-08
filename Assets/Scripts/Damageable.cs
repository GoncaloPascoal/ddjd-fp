using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] public Bar healthBar;

    protected void Start()
    {
        if (healthBar == null) return;
        OnMaxHealthChanged += () => healthBar.SetMaxValue(MaxHealth);
        OnHealthChanged += () => healthBar.SetValue(Health);
    }

    private int _maxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        set
        {
            _maxHealth = value;
            OnMaxHealthChanged.Invoke();
        }
    }

    private int _health;
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            OnHealthChanged.Invoke();
        }
    }

    public event Action OnMaxHealthChanged = delegate { }, OnHealthChanged = delegate { };

    public void ChangeHealth(int delta)
    {
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
        if (Health == 0) Die();
    }

    public void InitializeMaxHealth(int value)
    {
        _maxHealth = value;
        _health = value;

        if (healthBar != null)
        {
            healthBar.SetMaxValue(value);
            healthBar.SetValueInstantly(value);
        }
    }

    public void RestoreToMaxHealth()
    {
        Health = MaxHealth;
    }

    protected abstract void Die();
}
