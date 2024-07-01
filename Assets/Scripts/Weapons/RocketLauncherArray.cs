using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherArray : AWeapon
{
    [SerializeField]
    List<GameObject> rocketLaunchers;

    float rocketSpeed;

    float rocketDamage;

    [SerializeField]
    GameObject rocketProjectilePrefab;

    float rocketShotInterval;

    bool shooting;

    float lastRocketShotTime;

    [SerializeField]
    RocketLauncherArrayConfigSO rocketLauncherConfig;

    public RocketLauncherArrayConfigSO RocketLauncherConfig { get => rocketLauncherConfig; set {
        rocketLauncherConfig = value;
        rocketLauncherConfig.OnCurrentWeaponLevelChange.AddListener(UpdateWeaponConfig);
        UpdateWeaponConfig();
    }}

    public override WeaponConfigBaseSO WeaponConfig { get => RocketLauncherConfig; }


    // Start is called before the first frame update
    public void Start()
    {
        lastRocketShotTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            AttemptShootRocket();
        }
    }

    private void AttemptShootRocket()
    {
        float currentTime = Time.time;
        if ((currentTime - lastRocketShotTime) >= rocketShotInterval)
        {
            foreach (GameObject missileLauncher in rocketLaunchers)
            {
                ShootRocket(missileLauncher);
            }
            lastRocketShotTime = currentTime;
        }
    }

    public override void ShootBegin()
    {
        shooting = true;
    }

    public override void ShootEnd()
    {
        shooting = false;
    }

    public override void ShootInterrupt()
    {
        ShootEnd();
    }

    private void ShootRocket(GameObject missileLauncher)
    {
        GameObject projectile = Instantiate(rocketProjectilePrefab, missileLauncher.transform.position, missileLauncher.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        ExplosiveProjectileDamager projectileImpact = projectile.GetComponent<ExplosiveProjectileDamager>();
        projectileMovement.ProjectileSpeed = rocketSpeed;
        projectileImpact.ProjectileAreaDamage = rocketDamage;
    }

    public override bool IsActive()
    {
        return shooting;
    }

    public override void UpdateWeaponConfig() {
        RocketLauncherArrayLevelConfig levelConfig = rocketLauncherConfig.RocketLauncherArrayLevelConfigs[rocketLauncherConfig.CurrentWeaponLevel-1];
        rocketDamage = levelConfig.WeaponDamage;
        rocketSpeed = levelConfig.ProjectileSpeed;
        rocketShotInterval = levelConfig.RocketShotCooldown;
    }
}
