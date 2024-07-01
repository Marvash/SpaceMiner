using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannonArray : AWeapon
{
    [SerializeField]
    List<GameObject> LaserCannons;

    float laserSpeed;

    float laserDamage;

    [SerializeField]
    GameObject LaserProjectile;

    float laserShotInterval;

    EnergyBehaviour EnergyBehaviour;

    float energyCostPerShot;

    bool shootingLaser;

    float lastLaserShotTime;

    [SerializeField]
    LaserCannonArrayConfigSO laserCannonConfig;

    public LaserCannonArrayConfigSO LaserCannonConfig { get => laserCannonConfig; set {
        laserCannonConfig = value;
        laserCannonConfig.OnCurrentWeaponLevelChange.AddListener(UpdateWeaponConfig);
        UpdateWeaponConfig();
    }}

    public override WeaponConfigBaseSO WeaponConfig { get => LaserCannonConfig; }


    // Start is called before the first frame update
    public void Start()
    {
        lastLaserShotTime = 0.0f;
        EnergyBehaviour = PlayershipGO.GetComponent<EnergyBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootingLaser)
        {
            AttemptShootLasers();
        }
    }

    private void AttemptShootLasers()
    {
        if(EnergyBehaviour == null) {
            Debug.LogWarning("Attempting to shoot energy weapon with no energy behaviour set");
        }
        float currentTime = Time.time;
        if ((currentTime - lastLaserShotTime) >= laserShotInterval)
        {
            foreach(GameObject laserCannon in LaserCannons)
            {
                if(!laserCannon.activeSelf)
                    continue;
                if (EnergyBehaviour.ConsumeEnergy(energyCostPerShot) > 0.0f)
                {
                    ShootLaser(laserCannon);
                }
            }
            
            lastLaserShotTime = currentTime;
        }
    }

    public override void ShootBegin()
    {
        shootingLaser = true;
    }

    public override void ShootEnd()
    {
        shootingLaser = false;
    }

    public override void ShootInterrupt()
    {
        ShootEnd();
    }

    private void ShootLaser(GameObject laserCannon)
    {
        GameObject projectile = Instantiate(LaserProjectile, laserCannon.transform.position, laserCannon.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        SimpleProjectileDamager projectileImpact = projectile.GetComponent<SimpleProjectileDamager>();
        projectileMovement.ProjectileSpeed = laserSpeed;
        projectileImpact.ProjectileDamage = laserDamage;
    }

    public override bool IsActive()
    {
        return shootingLaser;
    }

    public void SetEnergyBehaviour(EnergyBehaviour energyBehaviour) {
        this.EnergyBehaviour = energyBehaviour;
    }

    public override void UpdateWeaponConfig() {
        LaserCannonArrayLevelConfig levelConfig = laserCannonConfig.LaserCannonArrayLevelConfigs[laserCannonConfig.CurrentWeaponLevel-1];
        laserDamage = levelConfig.WeaponDamage;
        laserSpeed = levelConfig.ProjectileSpeed;
        energyCostPerShot = levelConfig.EnergyCost;
        laserShotInterval = levelConfig.LaserShotCooldown;
        SetActiveCannonsByCount(levelConfig.NumCannons);
    }

    private void SetActiveCannonsByCount(int cannonCount) {
        foreach(GameObject cannon in LaserCannons) {
            cannon.SetActive(false);
        }
        switch(cannonCount) {
            case 1:
                LaserCannons[4].SetActive(true);
            break;
            case 2:
                LaserCannons[0].SetActive(true);
                LaserCannons[1].SetActive(true);
            break;
            case 3:
                LaserCannons[0].SetActive(true);
                LaserCannons[1].SetActive(true);
                LaserCannons[4].SetActive(true);
            break;
            case 4:
                LaserCannons[0].SetActive(true);
                LaserCannons[1].SetActive(true);
                LaserCannons[2].SetActive(true);
                LaserCannons[3].SetActive(true);
            break;
            default:
                LaserCannons[0].SetActive(true);
                LaserCannons[1].SetActive(true);
                LaserCannons[2].SetActive(true);
                LaserCannons[3].SetActive(true);
                LaserCannons[4].SetActive(true);
            break;
        }
    }
}
