using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableBoss : Damageable
{
    public override void Die()
    {
        gameObject.GetComponent<Boss>().ChangePhase();
    }
}
