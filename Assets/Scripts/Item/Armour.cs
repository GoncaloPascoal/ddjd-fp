using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armour : Item
{
    
    [SerializeField] private int maxHpIncrease = 30;
    private DamageablePlayer _damageable_player;

    void Start()
    {
        _damageable_player = GameObject.Find("PlayerArmature").GetComponent<DamageablePlayer>();
    }
    
    public override void Equip()
    {
        _damageable_player.ChangeMaxHealth(maxHpIncrease);
        Debug.Log("Max HP increase: " + maxHpIncrease);
        return;
    }
    
    public override void Unequip()
    {
        _damageable_player.ChangeMaxHealth(-maxHpIncrease);
        Debug.Log("Max HP decrease: " + maxHpIncrease);
        return;
    }

}
