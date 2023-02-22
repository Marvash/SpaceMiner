using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField]
    public PickupSpawnerSO PickupSpawnerSO;

    [SerializeField]
    public GameObject MiningDebriesPS;

    [SerializeField]
    public GameObject AsteroidDestructionPS;

    [SerializeField]
    public int AsteroidMiningHealth;

    [SerializeField]
    public int MaxMiningDrops;

    [SerializeField]
    public int MinMiningDrops;

    [SerializeField]
    public int MaxBreakingDrops;

    [SerializeField]
    public int MinBreakingDrops;

    [SerializeField]
    public string DropTableName;

    [SerializeField]
    DropTableLibrarySO DropTableLibrarySO;

    [SerializeField]
    public AsteroidDiffSO DiffManager;

    public int[] MiningDropTimings { get; set; }
    public int MiningDropTimingIndex { get; set; }
    public bool WasLoadedFromDiff { get; set; } = false;
    private string _asteroidId = "";

    public string AsteroidId { get => _asteroidId; set => _asteroidId = value; }

    public Droppable asteroidDropTable;

    private int miningDrops;
    private int breakingDrops;
    private Vector2 miningNormalDirection;
    private Vector2 miningPosition;

    private int _startingMiningHealth;

    // Start is called before the first frame update
    void Start()
    {
        asteroidDropTable = DropTableLibrarySO.getDropTable(DropTableName);
        breakingDrops = UnityEngine.Random.Range(MinBreakingDrops, MaxBreakingDrops);

        if (!WasLoadedFromDiff)
        {
            miningDrops = UnityEngine.Random.Range(MinMiningDrops, MaxMiningDrops);
            MiningDropTimings = new int[miningDrops];
            int miningDropHealthInterval = AsteroidMiningHealth / miningDrops;

            for (int i = 0; i < miningDrops; i++)
            {
                MiningDropTimings[i] = (miningDropHealthInterval * i) + UnityEngine.Random.Range(0, miningDropHealthInterval);
            }
            MiningDropTimingIndex = miningDrops - 1;
            _startingMiningHealth = AsteroidMiningHealth;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool HasChangedFromStartingState()
    {
        return _startingMiningHealth != AsteroidMiningHealth;
    }

    public void applyMiningDamage(int damage)
    {
        AsteroidMiningHealth -= damage;
        while (MiningDropTimingIndex >= 0 && (AsteroidMiningHealth <= (MiningDropTimings[MiningDropTimingIndex])))
        {
            GameObject droppedItem = dropRandomPickup(miningPosition);
            PickupStackScript valuablePickup = droppedItem.GetComponent<PickupStackScript>();
            if(valuablePickup)
            {
                float normalDirAngle = Mathf.Atan2(miningNormalDirection.y, miningNormalDirection.x) * Mathf.Rad2Deg;
                normalDirAngle += UnityEngine.Random.Range(-60, 61);
                float normalDirAngleRad = normalDirAngle * Mathf.Deg2Rad;
                Vector2 dropLerpDirection = new Vector2(Mathf.Cos(normalDirAngleRad), Mathf.Sin(normalDirAngleRad));
                Vector2 targetPos = miningPosition + (dropLerpDirection * 0.3f);
                valuablePickup.setLerpToTargetPosition(targetPos);
            }
            MiningDropTimingIndex--;
        }
        if(AsteroidMiningHealth <= 0)
        {
            for(int i = 0; i < breakingDrops; i++)
            {
                Vector2 v2Position = new Vector2(transform.position.x, transform.position.y);
                GameObject droppedItem = dropRandomPickup(v2Position);
                PickupStackScript valuablePickup = droppedItem.GetComponent<PickupStackScript>();
                if (valuablePickup)
                {
                    float normalDirAngle = Mathf.Atan2(miningNormalDirection.y, miningNormalDirection.x) * Mathf.Rad2Deg;
                    normalDirAngle += UnityEngine.Random.Range(-180, 181);
                    float normalDirAngleRad = normalDirAngle * Mathf.Deg2Rad;
                    Vector2 dropLerpDirection = new Vector2(Mathf.Cos(normalDirAngleRad), Mathf.Sin(normalDirAngleRad));
                    Vector2 targetPos = v2Position + (dropLerpDirection * 0.2f);
                    valuablePickup.setLerpToTargetPosition(targetPos);
                }
            }
            DestroyAsteroid();
        }
    }
    private GameObject dropRandomPickup(Vector2 dropPosition)
    {
        PickupStack droppedStack = asteroidDropTable.drop();
        GameObject selectedGO = null;
        if (droppedStack != null)
        {
            selectedGO = PickupSpawnerSO.spawnStandardPickup(droppedStack);
            selectedGO.transform.position = dropPosition;
        }
        return selectedGO;
    }
    public void updateMiningFX(Vector2 position, Vector2 direction)
    {
        miningNormalDirection = direction;
        miningPosition = position;
        float angle = Mathf.Atan2(miningNormalDirection.y, miningNormalDirection.x) * Mathf.Rad2Deg;
        MiningDebriesPS.transform.position = new Vector3(miningPosition.x, miningPosition.y, 0.0f);
        MiningDebriesPS.transform.eulerAngles = new Vector3(angle * -1.0f, 0.0f, 0.0f);
        MiningDebriesPS.SetActive(true);
        if (MiningDebriesPS.GetComponent<ParticleSystem>().isStopped) {
            MiningDebriesPS.GetComponent<ParticleSystem>().Play();
        }
    }

    public void stopMiningFX()
    {
        MiningDebriesPS.GetComponent<ParticleSystem>().Stop();
        MiningDebriesPS.SetActive(false);
    }

    public void DestroyAsteroid()
    {
        if(_asteroidId.Equals("")) {
            _asteroidId = Guid.NewGuid().ToString();
        }
        DiffManager.RegisterDestroyedAsteroid(AsteroidId);
        GameObject destructionPSInstance = Instantiate(AsteroidDestructionPS, transform.position, Quaternion.identity);
        destructionPSInstance.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}
