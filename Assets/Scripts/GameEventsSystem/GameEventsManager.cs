using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    [SerializeField]
    private GameManagerSO gameManager;

    [SerializeField]
    private GameEventsLayersSO GameEventsLayersSO;

    [SerializeField]
    private EnemyGroupProceduralSpawnerSO EnemyGroupProceduralSpawnerSO;

    private FastNoise _fastNoise = new FastNoise();

    private bool _gameEventsActive = false;

    private float _nextGameEvent = 0.0f;

    public float GameEventActivationDelay = 0.0f;

    public void ActivateGameEvents() {
        if(!_gameEventsActive) {
            _gameEventsActive = true;
            _nextGameEvent = Time.time + GameEventActivationDelay;
            Debug.Log("Activating game events");
        }
    }

    public void DeactivateGameEvents() {
        if(_gameEventsActive) {
            _gameEventsActive = false;
            Debug.Log("Deactivating game events");
        }
    }

    private void Update() {
        if(_gameEventsActive && Time.time >= _nextGameEvent) {
            selectNextEvent();
        }
    }

    private void selectNextEvent() {
        Debug.Log("Selecting next event");
        Vector2 playershipPos = gameManager.Player.transform.position;
        GameEventsLayersSO.Layer layer = pickGameEventLayer(playershipPos);
        if(layer == null) {
            return;
        }
        GameEventSO gameEventSO = layer.LayerObject;
        switch(gameEventSO.GameEvent) {
            case GameEvent.NONE:
                Debug.Log("No event selected");
                break;
            case GameEvent.ENEMY_SPAWN:
                Debug.Log("Enemy spawn event selected");
                EnemyGroupProceduralSpawnerSO.SpawnEnemyGroupByPosition(playershipPos);
                break;
        }
        _nextGameEvent = Time.time + computeNextGameEventTime(playershipPos, layer);
    }

    private GameEventsLayersSO.Layer pickGameEventLayer(Vector2 position) {
        float distanceFromCenter = position.magnitude;
        float totalWeights = 0.0f;
        Dictionary<GameEventsLayersSO.Layer, float> candidateLayers = new Dictionary<GameEventsLayersSO.Layer, float>();
        foreach (GameEventsLayersSO.Layer layer in GameEventsLayersSO.Layers)
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
        float randomValue = Utility.RemapRangeTo01(_fastNoise.GetValue(position.x + 4000.0f, position.y - 4000.0f), -1.0f, 1.0f);
        float currentInteravalMin = 0.0f;
        foreach (GameEventsLayersSO.Layer layer in candidateLayers.Keys)
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

    private float computeNextGameEventTime(Vector2 position, GameEventsLayersSO.Layer layer) {
        GameEventSO gameEventSO = layer.LayerObject;
        float distance = position.magnitude;
        float distanceScalingFactor = (distance - layer.MinDistance) / (layer.MaxDistance - layer.MinDistance);
        float minTime = gameEventSO.EndMinWaitTime * distanceScalingFactor + gameEventSO.StartMinWaitTime * (1.0f - distanceScalingFactor);
        float maxTime = gameEventSO.EndMaxWaitTime * distanceScalingFactor + gameEventSO.StartMaxWaitTime * (1.0f - distanceScalingFactor);
        Debug.Log("Min time " + minTime + " Max time " + maxTime);
        return Random.Range(minTime, maxTime);
    }
}
