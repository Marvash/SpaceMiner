using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DiffType
    {
        NONE,
        UPDATED,
        DESTROYED
    }

[CreateAssetMenu(fileName = "AsteroidDiffSO", menuName = "ScriptableObjects/AsteroidDiffSO", order = 1)]
public class AsteroidDiffSO : ScriptableObject
{
    private struct AsteroidDiff
    {
        public DiffType type;
        public int health;
        public int[] dropTimings;
        public int dropTimingIndex;
    }

    private Dictionary<string, AsteroidDiff> asteroidDiffMap = new Dictionary<string, AsteroidDiff>();

    public void SaveAsteroid(string id, GameObject asteroid)
    {
        AsteroidBehaviour asteroidBehaviour = asteroid.GetComponent<AsteroidBehaviour>();
        if(asteroidBehaviour)
        {
            if (asteroidBehaviour.HasChangedFromStartingState())
            {
                AsteroidDiff asteroidDiff = new AsteroidDiff();
                asteroidDiff.type = DiffType.UPDATED;
                asteroidDiff.health = asteroidBehaviour.AsteroidMiningHealth;
                asteroidDiff.dropTimings = asteroidBehaviour.MiningDropTimings;
                asteroidDiff.dropTimingIndex = asteroidBehaviour.MiningDropTimingIndex;
                Debug.Log("Added to diff " + asteroidDiff.health);
                asteroidDiffMap.Add(id, asteroidDiff);
            }
        }
    }

    public DiffType GetAsteroidDiffType(string id)
    {
        DiffType toRtn = DiffType.NONE;
        AsteroidDiff diff;
        if(asteroidDiffMap.TryGetValue(id, out diff))
        {
            toRtn = diff.type;
        }
        return toRtn;
    }

    public void LoadDiffToAsteroid(string id, GameObject asteroid)
    {
        AsteroidDiff diff;
        AsteroidBehaviour asteroidBehaviour;
        if(asteroidDiffMap.TryGetValue(id, out diff) && diff.type == DiffType.UPDATED)
        {
            asteroidBehaviour = asteroid.GetComponent<AsteroidBehaviour>();
            if (asteroidBehaviour)
            {
                asteroidBehaviour.AsteroidMiningHealth = diff.health;
                asteroidBehaviour.MiningDropTimings = diff.dropTimings;
                asteroidBehaviour.MiningDropTimingIndex = diff.dropTimingIndex;
                asteroidBehaviour.WasLoadedFromDiff = true;
                Debug.Log("Loaded diff " + asteroidBehaviour.AsteroidMiningHealth);
                asteroidDiffMap.Remove(id);
            }
        }
    }

    public void RegisterDestroyedAsteroid(string id)
    {
        AsteroidDiff diff = new AsteroidDiff();
        diff.type = DiffType.DESTROYED;
        asteroidDiffMap.Add(id, diff);
    }
}
