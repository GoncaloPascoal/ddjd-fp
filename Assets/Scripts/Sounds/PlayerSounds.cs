using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : EntitySounds
{
    [SerializeField] private int numberFootsteps = 5;
    [SerializeField] private int numberWeaponAttacks = 5;
    [SerializeField] private int numberRolls = 6;
    [SerializeField] private int numberSpells = 4;
    [SerializeField] private int numberJumps = 6;
    [SerializeField] private int numberBackstabs = 1;
    
    
    public void FootstepSound()
    {
        int randomSound = Random.Range(1, numberFootsteps + 1);
        
        Play3DSound("character/Footsteps/footsteps_" + randomSound);
    }

    public void JumpStartSound()
    {
        int randomSound = Random.Range(1, numberJumps + 1);
        
        Play3DSound("character/Jump/Jump_up_" + randomSound, 1);
    }

    public void JumpLandSound()
    {
        int randomSound = Random.Range(1, numberJumps + 1);
        
        Play3DSound("character/Jump/Jump_down_" + randomSound);
    }

    public void WeaponAttackSound()
    {
        int randomSound = Random.Range(1, numberWeaponAttacks + 1);
        
        Play3DSound("character/Blade_Slash/Slash_" + randomSound);
    }

    public void RollSound()
    {
        int randomSound = Random.Range(1, numberRolls + 1);
        
        Play3DSound("character/Dodge/Roll_" + randomSound);
    }

    public void PlayResurrectSound()
    {
        int randomSound = Random.Range(1, numberSpells + 1);
        
        Play3DSound("character/mind control/Spell_" + randomSound);
    }

    public void BackstabSound()
    {
        Play3DSound("character/Backstab");
    }
}
