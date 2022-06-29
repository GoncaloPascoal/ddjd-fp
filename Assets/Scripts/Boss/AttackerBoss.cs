using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerBoss : Attacker
{
    public void AttackBoss()
    {
        _isAttacking = true;
        weapon.Attack();
    }
}
