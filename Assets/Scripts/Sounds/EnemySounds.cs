using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySounds : EntitySounds
{
    [SerializeField] private int numberWeaponAttacks = 5;
    [SerializeField] private int numberFootsteps = 5;
    [SerializeField] private int numberArrow = 5;
    [SerializeField] private int numberDeath = 2;

    private List<String> parts = new List<string>{"body", "legs", "sword"};

public void WeaponAttackSound()
    {
        int randomSound = Random.Range(1, numberWeaponAttacks + 1);
        
        Play3DSound("character/Blade_Slash/Slash_" + randomSound);
    }
    
    public void FootstepSound()
    {
        int randomSound = Random.Range(1, numberFootsteps + 1);
        
        Play3DSound("character/Footsteps/footsteps_" + randomSound, 0.2f);
    }
    
    public void DeathSound()
    {
        int randomSound = Random.Range(1, numberDeath + 1);
        
        Play3DSound("Enemys/Dead/Morte_" + randomSound);
    }

    public void BodyFall()
    {
        int randomSound = Random.Range(1, numberDeath + 1);
        int randomPart = Random.Range(1, parts.Count + 1);
        
        Play3DSound("Enemys/Body_Fall/" + parts[randomPart] + "_" + randomSound);
    }

    public void ShootBow()
    {
        int randomSound = Random.Range(1, numberArrow + 1);
        
        Play3DSound("Enemys/Arrow/Arrow_" + randomSound);
    }
}
