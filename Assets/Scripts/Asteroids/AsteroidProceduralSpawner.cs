using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AsteroidProceduralSpawner : MonoBehaviour
{
    [SerializeField]
    private AsteroidFactory AsteroidFactory;

    [SerializeField]
    public float CenterSpawnDenialRadius;

    [SerializeField]
    private AsteroidDiffSO DiffManager;

    [SerializeField]
    private AsteroidSpawnLayersConfigSO AsteroidSpawnLayersConfigSO;

    private Dictionary<string, GameObject> asteroidMap = new Dictionary<string, GameObject>();

    private FastNoise _fastNoise = new FastNoise();

    public void SpawnAsteroidProcedural(Vector2 position, int xCellIndex, int yCellIndex)
    {
        if ((position - Vector2.zero).magnitude < CenterSpawnDenialRadius)
        {
            return;
        }
        string asteroidId = xCellIndex + "_" + yCellIndex;
        //Debug.Log("Added " + asteroidId);
        if (asteroidMap.ContainsKey(asteroidId))
        {
            return;
        }
        switch (DiffManager.GetAsteroidDiffType(asteroidId))
        {
            case DiffType.NONE:
                {
                    AsteroidSpawnLayersConfigSO.Layer layer = randomizeAsteroidLayer(position);
                    if (layer == null || !RandomizeAsteroidSpawnChance(position, layer))
                    {
                        return;
                    }
                    AsteroidType type = layer.LayerObject;
                    int asteroidTypeVariant = RandomizeAsteroidTypeVariant(type, position);
                    GameObject asteroid = AsteroidFactory.CreateAsteroid(type, position, UnityEngine.Random.Range(0.0f, 360.0f), asteroidTypeVariant);
                    asteroidMap.Add(asteroidId, asteroid);
                    AsteroidBehaviour asteroidBehaviour = asteroid.GetComponent<AsteroidBehaviour>();
                    asteroidBehaviour.AsteroidId = asteroidId;
                    break;
                }
            case DiffType.UPDATED:
                {
                    AsteroidSpawnLayersConfigSO.Layer layer = randomizeAsteroidLayer(position);
                    AsteroidType type = layer.LayerObject;
                    int asteroidTypeVariant = RandomizeAsteroidTypeVariant(type, position);
                    GameObject asteroid = AsteroidFactory.CreateAsteroid(type, position, UnityEngine.Random.Range(0.0f, 360.0f), asteroidTypeVariant);
                    DiffManager.LoadDiffToAsteroid(asteroidId, asteroid);
                    asteroidMap.Add(asteroidId, asteroid);
                    AsteroidBehaviour asteroidBehaviour = asteroid.GetComponent<AsteroidBehaviour>();
                    asteroidBehaviour.AsteroidId = asteroidId;
                    break;
                }
        }
        //Debug.Log("Adding " + asteroidId);
    }

    public void DespawnAsteroidByCellCoords(int xCellIndex, int yCellIndex)
    {
        string asteroidId = xCellIndex + "_" + yCellIndex;
        //Debug.Log("Deleting y asteroid " + asteroidId + " " + asteroidMap.ContainsKey(asteroidId));
        if (asteroidMap.ContainsKey(asteroidId) && (DiffManager.GetAsteroidDiffType(asteroidId) != DiffType.DESTROYED))
        {
            GameObject asteroid = asteroidMap[asteroidId];
            //Debug.Log("Asteroid despawn is null " + (asteroid == null));
            DiffManager.SaveAsteroid(asteroidId, asteroid);
            Destroy(asteroid);
            asteroidMap.Remove(asteroidId);
        }
    }

    private AsteroidSpawnLayersConfigSO.Layer randomizeAsteroidLayer(Vector2 position)
    {
        float distanceFromCenter = position.magnitude;
        float totalWeights = 0.0f;
        Dictionary<AsteroidSpawnLayersConfigSO.Layer, float> candidateLayers = new Dictionary<AsteroidSpawnLayersConfigSO.Layer, float>();
        foreach (AsteroidSpawnLayersConfigSO.Layer layer in AsteroidSpawnLayersConfigSO.Layers)
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
        float randomValue = Utility.RemapRangeTo01(_fastNoise.GetValue(position.x - 4000.0f, position.y + 4000.0f), -1.0f, 1.0f);
        float currentInteravalMin = 0.0f;
        foreach (AsteroidSpawnLayersConfigSO.Layer layer in candidateLayers.Keys)
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

    private int RandomizeAsteroidTypeVariant(AsteroidType type, Vector2 position)
    {
        int variantCount = AsteroidFactory.GetVariantCountByAsteroidType(type);
        _fastNoise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
        _fastNoise.SetFrequency(1.0f);
        int randomValue = Mathf.RoundToInt(Utility.RemapRangeTo01(_fastNoise.GetValue(position.x - 3000.0f, position.y + 3000.0f), -1.0f, 1.0f) * 1000);
        return randomValue % variantCount;
    }

    private bool RandomizeAsteroidSpawnChance(Vector2 position, AsteroidSpawnLayersConfigSO.Layer layer)
    {
        float distanceFromCenter = position.magnitude;
        if ((distanceFromCenter >= layer.MinDistance) && (distanceFromCenter <= layer.MaxDistance))
        {
            _fastNoise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
            _fastNoise.SetFrequency(1.0f);
            float randomValue = Utility.RemapRangeTo01(_fastNoise.GetValue(position.x + 6000.0f, position.y - 6000.0f), -1.0f, 1.0f);
            //float randomValue = Mathf.PerlinNoise(position.x + Seed + 6000.0f, position.y + Seed - 6000.0f);
            float distanceScalingFactor = (distanceFromCenter - layer.MinDistance) / (layer.MaxDistance - layer.MinDistance);
            float scaledChance = layer.MinSpawnChance + ((layer.MaxSpawnChance - layer.MinSpawnChance) * distanceScalingFactor);
            if (randomValue < scaledChance)
            {
                return true;
            }
        }
        return false;
    }
}
