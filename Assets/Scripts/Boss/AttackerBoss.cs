using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerBoss : Attacker
{
    public void AttackBoss()
    {
        Debug.Log("Attack boss");
        _isAttacking = true;
        weapon.Attack();
    }
}
