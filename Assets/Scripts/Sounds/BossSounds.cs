using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossSounds : EntitySounds
{
    [SerializeField] private int numberSwordAttacks = 4;
    [SerializeField] private int numberSlams = 3;
    [SerializeField] private int numberProjectile = 4;
    [SerializeField] private int numberFoot = 8;

    public void SwordAttackSound()
    {
        int randomSound = Random.Range(1, numberSwordAttacks + 1);
        
        Play3DSound("Boss/Sword/boss_sword_" + randomSound);
    }
    
    public void SwordSlamSound()
    {
        int randomSound = Random.Range(1, numberSlams + 1);
        
        Play3DSound("Boss/Sword_slam/slam_" + randomSound);
    }
    
    public void ProjectileSound()
    {
        int randomSound = Random.Range(1, numberProjectile + 1);
        Play3DSound("Boss/projectiles/projectile_" + randomSound);
    }

    public void FootStepSound()
    {
        int randomSound = Random.Range(1, numberFoot + 1);
        Play3DSound("Enemys/enemy_footsteps/Foot_" + randomSound);
    }
        
    public void SummonSound()
    {
        Play3DSound("Boss/Summons/Summon_spell");
    }
    
    public void ScreamSound()
    {
        Play3DSound("Boss/Boss_scream");
    }
}