using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherArray : IWeapon
{
    [SerializeField]
    private List<GameObject> MissileLaunchers;

    [SerializeField]
    public float MissileSpeed;

    [SerializeField]
    public float MissileDamage;

    [SerializeField]
    public GameObject MissileProjectile;

    [SerializeField]
    public float LaserShotInterval;

    private bool _shootingMissiles;

    private float _lastMissileShot;

    // Start is called before the first frame update
    void Start()
    {
        _lastMissileShot = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootingMissiles)
        {
            _attemptShootLasers();
        }
    }

    private void _attemptShootLasers()
    {
        float currentTime = Time.time;
        if ((currentTime - _lastMissileShot) >= LaserShotInterval)
        {
            foreach (GameObject missileLauncher in MissileLaunchers)
            {
                _shootLaser(missileLauncher);
            }
            _lastMissileShot = currentTime;
        }
    }

    public override void ShootBegin()
    {
        _shootingMissiles = true;
    }

    public override void ShootEnd()
    {
        _shootingMissiles = false;
    }

    public override void ShootInterrupt()
    {
        ShootEnd();
    }

    private void _shootLaser(GameObject missileLauncher)
    {
        GameObject projectile = Instantiate(MissileProjectile, missileLauncher.transform.position, missileLauncher.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        ExplosiveProjectileDamager projectileImpact = projectile.GetComponent<ExplosiveProjectileDamager>();
        projectileMovement.ProjectileSpeed = MissileSpeed;
        projectileImpact.ProjectileAreaDamage = MissileDamage;
    }

    public override bool IsActive()
    {
        return _shootingMissiles;
    }
}
