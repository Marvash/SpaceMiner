using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroupSO", menuName = "ScriptableObjects/EnemyGroupSO", order = 1)]
public class EnemyGroupSO : IEnemyGroup
{
    [SerializeField]
    private List<EnemyType> GroupEnemies;

    public override void SpawnEnemyGroup(EnemyFactorySO enemyFactory, Vector2 position) {
        foreach(EnemyType enemyType in GroupEnemies) {
            Vector2 randomOffsetVec2 = (Random.insideUnitCircle.normalized * 2);
            GameObject enemy = enemyFactory.SpawnEnemy(enemyType, position + randomOffsetVec2);
            IEnemy enemyBehaviour = enemy.GetComponent<IEnemy>();
            enemyBehaviour.ActivateEnemy();
        }
    }
}
