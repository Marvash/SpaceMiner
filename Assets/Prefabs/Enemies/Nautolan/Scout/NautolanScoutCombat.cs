using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NautolanScoutCombat : IDamageable
{
    public override void TakeDamage(float damage)
    {
        Health -= damage;
        if(Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
