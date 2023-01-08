using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFactory : MonoBehaviour
{
    [SerializeField]
    private PickupSpawner PickupSpawner;

    [SerializeField]
    private AsteroidDiffManager DiffManager;

    private Dictionary<AsteroidConfigSO.AsteroidType, AsteroidConfigSO> asteroidConfigLibrary = new Dictionary<AsteroidConfigSO.AsteroidType, AsteroidConfigSO>();

    private void Awake()
    {
        AsteroidConfigSO[] asteroidConfigList = Resources.LoadAll<AsteroidConfigSO>("AsteroidConfigs");
        foreach (AsteroidConfigSO asteroidConfig in asteroidConfigList)
        {
            asteroidConfigLibrary.Add(asteroidConfig.Type, asteroidConfig);
        }
    }

    public GameObject CreateAsteroid(AsteroidConfigSO.AsteroidType type)
    {
        return CreateAsteroid(type, Vector2.zero, 0.0f);
    }

    public GameObject CreateAsteroid(AsteroidConfigSO.AsteroidType type, Vector2 position)
    {
        return CreateAsteroid(type, position, 0.0f);
    }

    public GameObject CreateAsteroid(AsteroidConfigSO.AsteroidType type, Vector2 position, float angle)
    {
        return CreateAsteroid(type, position, angle, 0);
    }

    public GameObject CreateAsteroid(AsteroidConfigSO.AsteroidType type, Vector2 position, float angle, int asteroidVariant)
    {
        switch (type)
        {
            case AsteroidConfigSO.AsteroidType.COMMON:
                return CreateCommonAsteroid(position, angle, asteroidVariant);
            case AsteroidConfigSO.AsteroidType.UNCOMMON:
                return CreateUncommonAsteroid(position, angle, asteroidVariant);
            case AsteroidConfigSO.AsteroidType.RARE:
                return CreateRareAsteroid(position, angle, asteroidVariant);
            default:
                return null;
        }
    }

    private GameObject CreateCommonAsteroid(Vector2 position, float angle, int asteroidVariant)
    {
        GameObject asteroidPrefab = asteroidConfigLibrary[AsteroidConfigSO.AsteroidType.COMMON].VariantPrefabs[asteroidVariant];
        GameObject go = Instantiate(asteroidPrefab, position, Quaternion.Euler(0.0f, 0.0f, angle));
        AsteroidBehaviour asteroidBehaviour = go.GetComponent<AsteroidBehaviour>();
        asteroidBehaviour.PickupSpawner = PickupSpawner;
        asteroidBehaviour.DiffManager = DiffManager;
        return go;
    }

    private GameObject CreateUncommonAsteroid(Vector2 position, float angle, int asteroidVariant)
    {
        GameObject asteroidPrefab = asteroidConfigLibrary[AsteroidConfigSO.AsteroidType.UNCOMMON].VariantPrefabs[asteroidVariant];
        GameObject go = Instantiate(asteroidPrefab, position, Quaternion.Euler(0.0f, 0.0f, angle));
        AsteroidBehaviour asteroidBehaviour = go.GetComponent<AsteroidBehaviour>();
        asteroidBehaviour.PickupSpawner = PickupSpawner;
        asteroidBehaviour.DiffManager = DiffManager;
        return go;
    }

    private GameObject CreateRareAsteroid(Vector2 position, float angle, int asteroidVariant)
    {
        GameObject asteroidPrefab = asteroidConfigLibrary[AsteroidConfigSO.AsteroidType.RARE].VariantPrefabs[asteroidVariant];
        GameObject go = Instantiate(asteroidPrefab, position, Quaternion.Euler(0.0f, 0.0f, angle));
        AsteroidBehaviour asteroidBehaviour = go.GetComponent<AsteroidBehaviour>();
        asteroidBehaviour.PickupSpawner = PickupSpawner;
        asteroidBehaviour.DiffManager = DiffManager;
        return go;
    }

    public int GetVariantCountByAsteroidType(AsteroidConfigSO.AsteroidType type)
    {
        return asteroidConfigLibrary[type].VariantPrefabs.Count;
    }
}
