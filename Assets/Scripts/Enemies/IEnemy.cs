using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    NONE,
    NAUTOLAN_SCOUT,
    NAUTOLAN_FIGHTER,
    NAUTOLAN_SNIPER
}

public abstract class IEnemy : IDamageable
{
    protected virtual void Awake() {
        DeactivateEnemy();
    }
    public EnemyType EnemyType;
    public abstract void ActivateEnemy();
    public abstract void DeactivateEnemy();
    public abstract void InitializeEnemy(EnemyBehaviourInitializer enemyBehaviourInitializer);
}
