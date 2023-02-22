using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatBehaviour : IDamageable
{
    [SerializeField]
    private GameplayCanvasControllerSO gameplayCanvasControllerSO;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        gameplayCanvasControllerSO.UpdateHealth(_currentHealth / MaxHealth);
    }
}
