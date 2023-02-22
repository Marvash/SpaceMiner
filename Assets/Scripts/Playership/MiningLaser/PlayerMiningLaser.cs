using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMiningLaser : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    GameObject miningLaserOrigin;

    [SerializeField]
    GameObject miningLaserPSStart;

    [SerializeField]
    GameObject miningLaserPSTarget;

    [SerializeField]
    int miningDamage;

    [SerializeField]
    float miningDamageInterval;

    private LineRenderer miningLaserLineRenderer;
    private AsteroidBehaviour targetedAsteroid;
    private bool _shootingLaser = false;
    private float lastMiningDamageTiming;

    [SerializeField]
    float laserMaxLength;

    [SerializeField]
    float laserTickEnergyCost = 0.2f;

    [SerializeField]
    private EnergyBehaviour EnergyBehaviour;

    void Awake()
    {
        miningLaserLineRenderer = miningLaserOrigin.GetComponent<LineRenderer>();
        _shootingLaser = false;
        miningLaserLineRenderer.enabled = false;
        targetedAsteroid = null;
        lastMiningDamageTiming = Time.time;
        InputDispatcherSO.FireSecondaryStart += startShootMiningLaserHandler;
        InputDispatcherSO.FireSecondaryStop += stopShootMiningLaserHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if(_shootingLaser)
        {
            shootMiningLaser();
        } else
        {
            resetMiningLaser();
        }
    }

    private void shootMiningLaser()
    {
        Vector2 laserDir = new Vector2(miningLaserOrigin.transform.up.x, miningLaserOrigin.transform.up.y);
        Vector2 laserOrigin = new Vector2(miningLaserOrigin.transform.position.x, miningLaserOrigin.transform.position.y);
        int layerMask = 1 << 6;
        layerMask |= (1 << 8);
        RaycastHit2D rayHit = Physics2D.Raycast(laserOrigin, laserDir, laserMaxLength, layerMask);
        if (rayHit.collider != null)
        {
            if (EnergyBehaviour.ConsumeEnergy(laserTickEnergyCost) > 0.0f)
            {
                updateMiningLaserRay(laserDir, laserOrigin, rayHit);
                GameObject hitObj = rayHit.rigidbody.gameObject;
                Vector2 rayHitLocation = laserOrigin + (laserDir * rayHit.distance);
                AsteroidBehaviour asteroidBehaviour = hitObj.GetComponent<AsteroidBehaviour>();
                if (asteroidBehaviour)
                {
                    if (targetedAsteroid != null && asteroidBehaviour != targetedAsteroid)
                    {
                        targetedAsteroid.stopMiningFX();
                    }
                    targetedAsteroid = asteroidBehaviour;
                    targetedAsteroid.updateMiningFX(rayHitLocation, rayHit.normal);
                    float currentTime = Time.time;
                    if ((currentTime - lastMiningDamageTiming) >= miningDamageInterval)
                    {
                        targetedAsteroid.applyMiningDamage(miningDamage);
                        lastMiningDamageTiming = currentTime;
                    }
                }
            } else
            {
                resetMiningLaser();
            }
        }
        else
        {
            resetMiningLaser();
        }
    }

    private void updateMiningLaserRay(Vector2 laserDir, Vector2 laserOrigin, RaycastHit2D rayHit)
    {
        Vector2 rayHitLocationWithOffset = laserOrigin + (laserDir * (rayHit.distance));
        miningLaserLineRenderer.SetPosition(0, laserOrigin);
        miningLaserLineRenderer.SetPosition(1, rayHitLocationWithOffset);
        miningLaserLineRenderer.enabled = true;
        miningLaserPSStart.SetActive(true);
        Vector3 miningLaserPSTargetPos = miningLaserPSTarget.transform.localPosition;
        miningLaserPSTargetPos.y = miningLaserPSStart.transform.localPosition.y + rayHit.distance;
        miningLaserPSTarget.transform.localPosition = miningLaserPSTargetPos;
        float angle = Mathf.Atan2(rayHit.normal.y, rayHit.normal.x) * Mathf.Rad2Deg;
        miningLaserPSTarget.transform.eulerAngles = new Vector3(angle * -1.0f, 90.0f, 0.0f);
        miningLaserPSTarget.SetActive(true);
    }

    private void resetMiningLaser()
    {
        miningLaserLineRenderer.enabled = false;
        miningLaserPSStart.SetActive(false);
        miningLaserPSTarget.SetActive(false);
        if (targetedAsteroid)
        {
            targetedAsteroid.stopMiningFX();
            targetedAsteroid = null;
        }
    }

    private void startShootMiningLaserHandler()
    {
        _shootingLaser = true;
    }

    private void stopShootMiningLaserHandler()
    {
        _shootingLaser = false;
    }
}
