using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : EntitySounds
{
    [SerializeField] private int numberWeaponAttacks = 5;
    [SerializeField] private int numberFootsteps = 5;
    
    public void WeaponAttackSound()
    {
        int randomSound = Random.Range(1, numberWeaponAttacks + 1);
        
        Play3DSound("character/Blade_Slash/Slash_" + randomSound);
    }
    
    public void FootstepSound()
    {
        int randomSound = Random.Range(1, numberFootsteps + 1);
        
        Play3DSound("character/Footsteps/footsteps_" + randomSound);
    }

    public void ShootBow()
    {
        
    }
}
