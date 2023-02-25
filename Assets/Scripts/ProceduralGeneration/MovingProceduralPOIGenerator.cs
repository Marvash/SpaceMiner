using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MovingProceduralPOIGenerator : MonoBehaviour
{
    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    [SerializeField]
    private float GridCellSize;

    [SerializeField]
    private float SpawnUpdateInterval;

    [SerializeField]
    private float SpawnRadius;

    [SerializeField]
    private float XJitterMaxOffset;

    [SerializeField]
    private float YJitterMaxOffset;

    [SerializeField]
    private float DespawnUpdateInterval;

    [SerializeField]
    private float DespawnRadius;

    private Transform _target;

    private int CurrentSpawnXIndex;
    private int CurrentSpawnYIndex;
    private int CurrentDespawnXIndex;
    private int CurrentDespawnYIndex;

    private int _seed = 0;

    private bool _shouldRandomizeSeedAtStart = true;

    private int _maxSeedRange = 1000000;

    private FastNoise _fastNoise = new FastNoise();

    public int Seed { get => _seed; set => setSeed(value); }

    [SerializeField]
    private UnityEvent<Vector2, int, int> SpawnEvent = new UnityEvent<Vector2, int, int>();
    
    [SerializeField]
    private UnityEvent<int, int> DespawnEvent = new UnityEvent<int, int>();

    // Start is called before the first frame update
    public virtual void Start()
    {
        _target = PlayershipManagerSO.Player.transform;
        CurrentSpawnXIndex = Mathf.FloorToInt(_target.position.x / GridCellSize) - Mathf.CeilToInt((SpawnRadius / GridCellSize) * 2.0f);
        CurrentSpawnYIndex = Mathf.FloorToInt(_target.position.y / GridCellSize) - Mathf.CeilToInt((SpawnRadius / GridCellSize) * 2.0f);
        if (_shouldRandomizeSeedAtStart)
        {
            randomizeSeed();
        }
        StartCoroutine(spawnCoroutine());
        CurrentDespawnXIndex = Mathf.FloorToInt(_target.position.x / GridCellSize);
        CurrentDespawnYIndex = Mathf.FloorToInt(_target.position.y / GridCellSize);
        StartCoroutine(despawnCoroutine());
    }

    public virtual void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void randomizeSeed()
    {
        Seed = Mathf.RoundToInt(Random.value * _maxSeedRange);
    }

    private void setSeed(int seed)
    {
        _seed = seed;
        _shouldRandomizeSeedAtStart = false;
        _fastNoise.SetSeed(_seed);

    }

    private IEnumerator spawnCoroutine()
    {
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(SpawnUpdateInterval);
        }
    }

    protected void Spawn()
    {
        //Debug.Log("Generating asteroids...");
        int newXIndex = Mathf.FloorToInt(_target.position.x / GridCellSize);
        int newYIndex = Mathf.FloorToInt(_target.position.y / GridCellSize);
        int xSteps = newXIndex - CurrentSpawnXIndex;
        int ySteps = newYIndex - CurrentSpawnYIndex;
        //Debug.Log("xSteps: " + xSteps);
        //Debug.Log("ySteps: " + ySteps);
        if (xSteps == 0 && ySteps == 0)
        {
            return;
        }
        int halfGridExtent = Mathf.CeilToInt(SpawnRadius / GridCellSize);
        int newRightGridExtent = newXIndex + halfGridExtent;
        int newTopGridExtent = newYIndex + halfGridExtent;
        int newLeftGridExtent = newXIndex - halfGridExtent;
        int newBottomGridExtent = newYIndex - halfGridExtent;
        //Debug.Log("xMin: " + newLeftGridExtent + " xMax: " + newRightGridExtent);
        //Debug.Log("yMin: " + newBottomGridExtent + " yMax: " + newTopGridExtent);
        int xStepsSign = Mathf.RoundToInt(Mathf.Sign(xSteps));
        int yStepsSign = Mathf.RoundToInt(Mathf.Sign(ySteps));
        xSteps = Mathf.Abs(xSteps);
        ySteps = Mathf.Abs(ySteps);
        int genRightStart = newRightGridExtent - xSteps;
        int genLeftStart = newLeftGridExtent;
        int genTopStart = newTopGridExtent - ySteps;
        int genBottomStart = newBottomGridExtent;
        int addedAsteroidCount = 0;
        if (xSteps > 0)
        {
            for (int i = 0; i < xSteps; i++)
            {
                int xCellIndex = xStepsSign > 0 ? genRightStart + i : genLeftStart + i;
                for (int j = 0; j < (halfGridExtent * 2); j++)
                {
                    //Debug.Log("Generating x step");
                    int yCellIndex = genBottomStart + j;
                    float xCellPos = xCellIndex * GridCellSize;
                    float yCellPos = yCellIndex * GridCellSize;
                    //float _noiseMapShift = Mathf.PerlinNoise(_seed + xCellPos + 550.5f, _seed - yCellPos - 550.5f);
                    _fastNoise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
                    _fastNoise.SetFrequency(1.0f);
                    float _noiseMapShift = Utility.RemapRangeTo01(_fastNoise.GetValue(xCellPos + 550.5f, yCellPos - 550.5f), -1.0f, 1.0f);
                    float xCellCenter = xCellPos + (GridCellSize / 2.0f) + _noiseMapShift;
                    float yCellCenter = yCellPos + (GridCellSize / 2.0f) + _noiseMapShift;
                    //Debug.Log("xCellCenter: " + xCellPos + " yCellCenter: " + yCellPos);
                    //float xNoise = Mathf.PerlinNoise(xCellCenter + Seed, yCellCenter + Seed);
                    float xNoise = Utility.RemapRangeTo01(_fastNoise.GetValue(xCellCenter, yCellCenter), -1.0f, 1.0f);
                    float yNoise = Utility.RemapRangeTo01(_fastNoise.GetValue(xCellCenter + 1000, yCellCenter + 1000), -1.0f, 1.0f);
                    float xOffset = (GridCellSize * xNoise) - (GridCellSize / 2.0f);
                    float yOffset = (GridCellSize * yNoise) - (GridCellSize / 2.0f);
                    Vector2 finalPosition = new Vector2(xCellCenter + xOffset, yCellCenter + yOffset);
                    SpawnEvent.Invoke(finalPosition, xCellIndex, yCellIndex);
                    addedAsteroidCount++;
                }
            }
            CurrentSpawnXIndex = newXIndex;
        }
        if (ySteps > 0)
        {
            for (int i = 0; i < ySteps; i++)
            {
                int yCellIndex = yStepsSign > 0 ? genTopStart + i : genBottomStart + i;
                for (int j = 0; j < (halfGridExtent * 2) - xSteps; j++)
                {
                    //Debug.Log("Generating y step");
                    int xCellIndex = genLeftStart + j;
                    if ((xSteps > 0) && (xStepsSign < 0))
                    {
                        xCellIndex += xSteps;
                    }
                    float yCellPos = yCellIndex * GridCellSize;
                    float xCellPos = xCellIndex * GridCellSize;
                    //float _noiseMapShift = Mathf.PerlinNoise(_seed + xCellPos + 550.5f, _seed - yCellPos - 550.5f);
                    _fastNoise.SetNoiseType(FastNoise.NoiseType.WhiteNoise);
                    _fastNoise.SetFrequency(1.0f);
                    float _noiseMapShift = Utility.RemapRangeTo01(_fastNoise.GetValue(xCellPos + 550.5f, yCellPos - 550.5f), -1.0f, 1.0f);
                    float xCellCenter = xCellPos + (GridCellSize / 2.0f) + _noiseMapShift;
                    float yCellCenter = yCellPos + (GridCellSize / 2.0f) + _noiseMapShift;
                    //Debug.Log("xCellCenter: " + xCellPos + " yCellCenter: " + yCellPos);
                    //float xNoise = Mathf.PerlinNoise(xCellCenter + Seed, yCellCenter + Seed);
                    float xNoise = Utility.RemapRangeTo01(_fastNoise.GetValue(xCellCenter, yCellCenter), -1.0f, 1.0f);
                    float yNoise = Utility.RemapRangeTo01(_fastNoise.GetValue(xCellCenter + 1000, yCellCenter + 1000), -1.0f, 1.0f);
                    float xOffset = (GridCellSize * xNoise) - (GridCellSize / 2.0f);
                    float yOffset = (GridCellSize * yNoise) - (GridCellSize / 2.0f);
                    Vector2 finalPosition = new Vector2(xCellCenter + xOffset, yCellCenter + yOffset);
                    SpawnEvent.Invoke(finalPosition, xCellIndex, yCellIndex);
                    addedAsteroidCount++;
                }
            }
            CurrentSpawnYIndex = newYIndex;
        }
        //Debug.Log("Asteroids added " + addedAsteroidCount);
    }

    private IEnumerator despawnCoroutine()
    {
        while (true)
        {
            Despawn();
            yield return new WaitForSeconds(DespawnUpdateInterval);
        }
    }

    private void Despawn()
    {
        int newDespawnXIndex = Mathf.FloorToInt(_target.position.x / GridCellSize);
        int newDespawnYIndex = Mathf.FloorToInt(_target.position.y / GridCellSize);
        int xSteps = newDespawnXIndex - CurrentDespawnXIndex;
        int ySteps = newDespawnYIndex - CurrentDespawnYIndex;
        //Debug.Log("DESP xSteps: " + xSteps);
        //Debug.Log("DESP ySteps: " + ySteps);
        if (xSteps == 0 && ySteps == 0)
        {
            return;
        }
        int halfGridExtent = Mathf.CeilToInt(DespawnRadius / GridCellSize);
        int oldDespawnRightGridExtent = CurrentDespawnXIndex + halfGridExtent;
        int oldDespawnTopGridExtent = CurrentDespawnYIndex + halfGridExtent;
        int oldDespawnLeftGridExtent = CurrentDespawnXIndex - halfGridExtent;
        int oldDespawnBottomGridExtent = CurrentDespawnYIndex - halfGridExtent;
        //Debug.Log("DESP xMin: " + oldDespawnLeftGridExtent + " xMax: " + oldDespawnRightGridExtent);
        //Debug.Log("DESP yMin: " + oldDespawnBottomGridExtent + " yMax: " + oldDespawnTopGridExtent);
        int xStepsSign = Mathf.RoundToInt(Mathf.Sign(xSteps));
        int yStepsSign = Mathf.RoundToInt(Mathf.Sign(ySteps));
        xSteps = Mathf.Abs(xSteps);
        ySteps = Mathf.Abs(ySteps);
        int despawnRightStart = oldDespawnRightGridExtent - xSteps;
        int despawnLeftStart = oldDespawnLeftGridExtent;
        int despawnTopStart = oldDespawnTopGridExtent - ySteps;
        int despawnBottomStart = oldDespawnBottomGridExtent;
        if (xSteps > 0)
        {
            for (int i = 0; i < xSteps; i++)
            {
                int xCellIndex = xStepsSign > 0 ? despawnLeftStart + i : despawnRightStart + i;
                for (int j = 0; j < (halfGridExtent * 2); j++)
                {
                    int yCellIndex = oldDespawnBottomGridExtent + j;
                    DespawnEvent.Invoke(xCellIndex, yCellIndex);

                }
            }
            CurrentDespawnXIndex = newDespawnXIndex;
        }
        if (ySteps > 0)
        {
            for (int i = 0; i < ySteps; i++)
            {
                int yCellIndex = yStepsSign > 0 ? despawnBottomStart + i : despawnTopStart + i;
                for (int j = 0; j < (halfGridExtent * 2) - xSteps; j++)
                {
                    int xCellIndex = oldDespawnLeftGridExtent + j;
                    if ((xSteps > 0) && (xStepsSign > 0))
                    {
                        xCellIndex += xSteps;
                    }
                    DespawnEvent.Invoke(xCellIndex, yCellIndex);
                }
            }
            CurrentDespawnYIndex = newDespawnYIndex;
        }
        //Debug.Log("Despawning asteroids... " + asteroidMap.Count);
    }
}

