using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehaviour : MonoBehaviour
{
    [SerializeField]
    public PickupSpawner pickupSpawner;

    [SerializeField]
    public GameObject miningPS;

    [SerializeField]
    public GameObject asteroidDestructionPS;

    [SerializeField]
    public int asteroidMiningHealth;

    [SerializeField]
    public int maxMiningDrops;

    [SerializeField]
    public int minMiningDrops;

    [SerializeField]
    public int maxBreakingDrops;

    [SerializeField]
    public int minBreakingDrops;

    [SerializeField]
    DropTableLibraryScriptableObject dropTableLibrarySO;

    public Droppable asteroidDropTable;

    private int miningDrops;
    private int breakingDrops;
    private int[] miningDropTimings;
    private int miningDropTimingIndex;
    private Vector2 miningNormalDirection;
    private Vector2 miningPosition;

    // Start is called before the first frame update
    void Start()
    {
        asteroidDropTable = dropTableLibrarySO.getDropTable("AsteroidDropTable");

        miningDrops = Random.Range(minMiningDrops, maxMiningDrops);
        breakingDrops = Random.Range(minBreakingDrops, maxBreakingDrops);
        miningDropTimings = new int[miningDrops];
        int miningDropHealthInterval = asteroidMiningHealth / miningDrops;

        for (int i = 0; i < miningDrops; i++)
        {
            miningDropTimings[i] = (miningDropHealthInterval * i) + Random.Range(0, miningDropHealthInterval);
        }
        miningDropTimingIndex = miningDrops - 1;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == 9)
        {
            Destroy(other);
        }
    }
    public void applyMiningDamage(int damage)
    {
        asteroidMiningHealth -= damage;
        while (miningDropTimingIndex >= 0 && (asteroidMiningHealth <= (miningDropTimings[miningDropTimingIndex])))
        {
            GameObject droppedItem = dropRandomPickup(miningPosition);
            PickupStackScript valuablePickup = droppedItem.GetComponent<PickupStackScript>();
            if(valuablePickup)
            {
                float normalDirAngle = Mathf.Atan2(miningNormalDirection.y, miningNormalDirection.x) * Mathf.Rad2Deg;
                normalDirAngle += Random.Range(-60, 61);
                float normalDirAngleRad = normalDirAngle * Mathf.Deg2Rad;
                Vector2 dropLerpDirection = new Vector2(Mathf.Cos(normalDirAngleRad), Mathf.Sin(normalDirAngleRad));
                Vector2 targetPos = miningPosition + (dropLerpDirection * 0.3f);
                valuablePickup.setLerpToTargetPosition(targetPos);
            }
            miningDropTimingIndex--;
        }
        if(asteroidMiningHealth <= 0)
        {
            for(int i = 0; i < breakingDrops; i++)
            {
                Vector2 v2Position = new Vector2(transform.position.x, transform.position.y);
                GameObject droppedItem = dropRandomPickup(v2Position);
                PickupStackScript valuablePickup = droppedItem.GetComponent<PickupStackScript>();
                if (valuablePickup)
                {
                    float normalDirAngle = Mathf.Atan2(miningNormalDirection.y, miningNormalDirection.x) * Mathf.Rad2Deg;
                    normalDirAngle += Random.Range(-180, 181);
                    float normalDirAngleRad = normalDirAngle * Mathf.Deg2Rad;
                    Vector2 dropLerpDirection = new Vector2(Mathf.Cos(normalDirAngleRad), Mathf.Sin(normalDirAngleRad));
                    Vector2 targetPos = v2Position + (dropLerpDirection * 0.2f);
                    valuablePickup.setLerpToTargetPosition(targetPos);
                }
            }
            GameObject destructionPSInstance = Instantiate(asteroidDestructionPS, transform.position, Quaternion.identity);
            destructionPSInstance.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject);
        }
    }
    private GameObject dropRandomPickup(Vector2 dropPosition)
    {
        PickupStack droppedStack = asteroidDropTable.drop();
        GameObject selectedGO = null;
        if (droppedStack != null)
        {
            selectedGO = pickupSpawner.spawnStandardPickup(droppedStack);
            selectedGO.transform.position = dropPosition;
        }
        return selectedGO;
    }
    public void updateMiningFX(Vector2 position, Vector2 direction)
    {
        miningNormalDirection = direction;
        miningPosition = position;
        float angle = Mathf.Atan2(miningNormalDirection.y, miningNormalDirection.x) * Mathf.Rad2Deg;
        miningPS.transform.position = new Vector3(miningPosition.x, miningPosition.y, 0.0f);
        miningPS.transform.eulerAngles = new Vector3(angle * -1.0f, 0.0f, 0.0f);
        miningPS.SetActive(true);
        if (miningPS.GetComponent<ParticleSystem>().isStopped) {
            miningPS.GetComponent<ParticleSystem>().Play();
        }
    }

    public void stopMiningFX()
    {
        miningPS.GetComponent<ParticleSystem>().Stop();
        miningPS.SetActive(false);
    }
}
