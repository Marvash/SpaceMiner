using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyFactorySO", menuName = "ScriptableObjects/EnemyFactorySO", order = 1)]
public class EnemyFactorySO : ScriptableObject
{
    [SerializeField]
    private List<GameObject> EnemyPrefabs = new List<GameObject>();

    private Dictionary<EnemyType, GameObject> _enemyPrefabsMap = new Dictionary<EnemyType, GameObject>();

    [SerializeField]
    GameManagerSO gameManager;

    private EnemyBehaviourInitializer _enemyBehaviourInitializer;

    void OnEnable() {
        foreach(GameObject enemy in EnemyPrefabs) {
            _enemyPrefabsMap.Add(enemy.GetComponent<IEnemy>().EnemyType, enemy);
        }
        _enemyBehaviourInitializer = new EnemyBehaviourInitializer(gameManager);
    }

    public GameObject SpawnEnemy(EnemyType enemyType) {
        return SpawnEnemy(enemyType, Vector2.zero);
    }

    public GameObject SpawnEnemy(EnemyType enemyType, Vector2 position) {
        GameObject enemy = null;

        if(_enemyPrefabsMap.Count == 0) {
            Debug.LogWarning("No enemy prefabs available");
            return enemy;
        }

        GameObject prefab = _enemyPrefabsMap[enemyType];
        if(prefab != null) {
            enemy = Instantiate(prefab, position, prefab.transform.rotation);
            IEnemy enemyBehaviour = enemy.GetComponent<IEnemy>();
            if(enemyBehaviour != null) {
                enemyBehaviour.InitializeEnemy(_enemyBehaviourInitializer);
            }
        } else {
            Debug.LogWarning("No enemy prefabs available with type " + enemyType);
        }
        return enemy;
    }
}
