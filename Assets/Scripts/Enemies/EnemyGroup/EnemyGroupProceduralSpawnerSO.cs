using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyGroupProceduralSpawnerSO", menuName = "ScriptableObjects/EnemyGroupProceduralSpawnerSO", order = 3)]
public class EnemyGroupProceduralSpawnerSO : ScriptableObject
{
    [SerializeField]
    private MoneyPlayerDataSO PlayershipManagerSO;

    [SerializeField]
    private EnemyFactorySO EnemyFactorySO;

    [SerializeField]
    private EnemyGroupSpawnLayersConfigSO EnemyGroupSpawnLayersConfigSO;

    public float EnemyMinSpawnRange = 10.0f;
    public float EnemyMaxSpawnRange = 15.0f;

    private FastNoise _fastNoise = new FastNoise();

    public void SpawnEnemyGroupByPosition(Vector2 position) {
        EnemyGroupSpawnLayersConfigSO.Layer layer = randomizeEnemyGroupLayer(position);
        Debug.Log("Layer is " + layer);
        if(layer == null) {
            return;
        }
        Debug.Log("Spawning group " + layer.LayerObject.GroupName);
        Vector2 spawnPosition = position + Random.insideUnitCircle.normalized * 10.0f;
        EnemyGroupSO enemyGroup = layer.LayerObject;
        enemyGroup.SpawnEnemyGroup(EnemyFactorySO, spawnPosition);
    }

    private EnemyGroupSpawnLayersConfigSO.Layer randomizeEnemyGroupLayer(Vector2 position)
    {
        float distanceFromCenter = position.magnitude;
        float totalWeights = 0.0f;
        Dictionary<EnemyGroupSpawnLayersConfigSO.Layer, float> candidateLayers = new Dictionary<EnemyGroupSpawnLayersConfigSO.Layer, float>();
        foreach (EnemyGroupSpawnLayersConfigSO.Layer layer in EnemyGroupSpawnLayersConfigSO.Layers)
        {
            if ((distanceFromCenter >= layer.MinDistance) && (distanceFromCenter <= layer.MaxDistance))
            {
                float distanceScalingFactor = (distanceFromCenter - layer.MinDistance) / (layer.MaxDistance - layer.MinDistance);
                float scaledWeight = layer.MinSelectionWeight + (layer.MaxSelectionWeight - layer.MinSelectionWeight) * distanceScalingFactor;
                totalWeights += scaledWeight;
                candidateLayers.Add(layer, scaledWeight);
            }
        }
        //float randomValue = Mathf.PerlinNoise(position.x + Seed + 4000.0f, position.y + Seed - 4000.0f);
        _fastNoise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
        _fastNoise.SetFrequency(1.0f);
        float randomValue = Utility.RemapRangeTo01(_fastNoise.GetValue(position.x - 5000.0f, position.y + 5000.0f), -1.0f, 1.0f);
        float currentInteravalMin = 0.0f;
        foreach (EnemyGroupSpawnLayersConfigSO.Layer layer in candidateLayers.Keys)
        {
            float currentWeightNorm = candidateLayers[layer] / totalWeights;
            float currentIntervalMax = currentInteravalMin + currentWeightNorm;

            if ((randomValue >= currentInteravalMin) && (randomValue <= currentIntervalMax))
            {
                return layer;
            }
            currentInteravalMin = currentIntervalMax;
        }

        return null;
    }
}
