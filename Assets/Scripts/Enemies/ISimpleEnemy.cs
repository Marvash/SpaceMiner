using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISimpleEnemy : IEnemy
{
    protected GameObject _target;
    public abstract void SetTarget(GameObject target);
    public override void InitializeEnemy(EnemyBehaviourInitializer enemyBehaviourInitializer) {
        enemyBehaviourInitializer.InitSimpleEnemy(this);
    }
}
