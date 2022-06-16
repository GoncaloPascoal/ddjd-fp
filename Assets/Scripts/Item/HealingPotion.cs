using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPotion : Potion
{
    [SerializeField] private float healingPercent = 0.4f;
    private DamageablePlayer _damageable_player;

    void Start()
    {
        _damageable_player = GameObject.Find("PlayerArmature").GetComponent<DamageablePlayer>();
    }
    public bool CanUse()
    {
        return true; //_damageable_player.Health != _damageable_player.MaxHealth;
    }
    
    public override bool Use()
    {
        if (!CanUse())
            return false;

        _damageable_player.ChangeHealth((int)(_damageable_player.Health - healingPercent * _damageable_player.MaxHealth));
        Destroy(gameObject);
        return true;
    }
    
}
