using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [SerializeField] public Bar healthBar;
    private ThirdPersonController _player;

    protected virtual void Start()
    {
        if (healthBar == null) return;
        _player = gameObject.GetComponent<ThirdPersonController>();
        OnMaxHealthChanged += () => healthBar.SetMaxValue(MaxHealth);
        OnHealthChanged += () => healthBar.SetValue(Health);
    }

    private int _maxHealth;
    public int MaxHealth
    {
        get => _maxHealth;
        protected set
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
        if (_player != null && _player.IsRolling()) return;
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
        if (Health == 0) Die();
    }

    public void AlwaysChangeHealth(int delta)
    {
        Health = Mathf.Clamp(Health + delta, 0, MaxHealth);
        if (Health == 0) Die();
    }

    public void ChangeMaxHealth(int delta)
    {
        MaxHealth = Mathf.Clamp(MaxHealth + delta, 0, MaxHealth);
        Health = Mathf.Clamp(Health, 0, MaxHealth);
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
