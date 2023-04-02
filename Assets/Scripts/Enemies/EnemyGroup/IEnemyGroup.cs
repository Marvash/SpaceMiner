using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnemyGroup : ScriptableObject
{
    public string GroupName;

    public abstract void SpawnEnemyGroup(EnemyFactorySO enemyFactory, Vector2 position);
}
