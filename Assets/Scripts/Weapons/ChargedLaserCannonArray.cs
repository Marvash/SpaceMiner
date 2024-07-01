using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargedLaserCannonArray : AWeapon
{
    [SerializeField]
    List<GameObject> laserCannons;
    float laserMaxSpeed;
    float laserMinSpeed;
    float laserMaxDamage;
    float laserMinDamage;
    [SerializeField]
    GameObject laserProjectile;
    float laserShotMaxCharge;
    float laserShotMinCharge;
    float laserShotEnergyAllocationRate = 0.05f;
    private EnergyBehaviour EnergyBehaviour;
    private float EnergyAllocatedPerTick = 0.5f;
    private bool _shootingLaser;
    private bool requiresEnergy;
    private bool charging = false;
    private float energyAllocated = 0.0f;

    public UnityEvent ChargingStart = new UnityEvent();
    public UnityEvent ChargingStop = new UnityEvent();
    public UnityEvent ChargingComplete = new UnityEvent();
    public UnityEvent LaserShot = new UnityEvent();

    [SerializeField]
    LaserCannonArrayConfigSO config;

    public override WeaponConfigBaseSO WeaponConfig { get => config; }
    
    // Update is called once per frame
    void Update()
    {
        if (_shootingLaser)
        {
            if(!charging)
            {
                InvokeRepeating("AllocateEnergy", 0.0f, laserShotEnergyAllocationRate);
                ChargingStart.Invoke();
                charging = true;
            }
        } else if(charging)
        {
            AttemptShootLasers();
            InterruptCharge();
        }
    }

    private void AllocateEnergy()
    {
        
        float diff = laserShotMaxCharge - energyAllocated;
        if (diff > 0.0f)
        {
            if (requiresEnergy)
            {
                energyAllocated += EnergyBehaviour.AllocateEnergy(Mathf.Min(diff, EnergyAllocatedPerTick));
            } else
            {
                energyAllocated += Mathf.Min(diff, EnergyAllocatedPerTick);
            }

        }
        else
        {
            ChargingComplete.Invoke();
        }

    }

    private void AttemptShootLasers()
    {
        if (energyAllocated >= laserShotMinCharge)
        {
            if (requiresEnergy)
            {
                EnergyBehaviour.ConsumeAllocatedEnergy();
            }
            float consumedEnergyPercentage = energyAllocated / laserShotMaxCharge;
            foreach (GameObject laserCannon in laserCannons)
            {
                ShootChargedLaser(laserCannon, consumedEnergyPercentage);
            }
        }
    }

    public override void ShootBegin()
    {
        _shootingLaser = true;
    }

    public override void ShootEnd()
    {
        _shootingLaser = false;
    }

    public override void ShootInterrupt()
    {
        ShootEnd();
    }

    public float GetCurrentProjectileSpeed()
    {
        float consumedEnergyPercentage = energyAllocated / laserShotMaxCharge;
        return laserMinSpeed + (laserMaxSpeed - laserMinSpeed) * consumedEnergyPercentage;
    }

    private void InterruptCharge()
    {
        if (charging)
        {
            CancelInvoke();
            ChargingStop.Invoke();
            if (requiresEnergy)
            {
                EnergyBehaviour.ResetAllocatedEnergy();
            }
            energyAllocated = 0.0f;
            charging = false;
        }
    }

    public float GetChargePercentage()
    {
        return energyAllocated / laserShotMaxCharge;
    }

    private void ShootChargedLaser(GameObject laserCannon, float chargePercentage)
    {
        GameObject projectile = Instantiate(laserProjectile, laserCannon.transform.position, laserCannon.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        SimpleProjectileDamager projectileImpact = projectile.GetComponent<SimpleProjectileDamager>();
        projectileMovement.ProjectileSpeed = laserMinSpeed + (laserMaxSpeed - laserMinSpeed) * chargePercentage;
        projectileImpact.ProjectileDamage = laserMinDamage + (laserMaxDamage - laserMinDamage) * chargePercentage;
    }

    public override bool IsActive()
    {
        return _shootingLaser;
    }

    public void SetEnergyBehaviour(EnergyBehaviour energyBehaviour) {
        this.EnergyBehaviour = energyBehaviour;
    }
    public override void UpdateWeaponConfig() {

    }
}
