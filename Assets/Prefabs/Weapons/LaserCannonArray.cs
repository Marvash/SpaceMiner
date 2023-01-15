using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannonArray : IWeapon
{
    [SerializeField]
    private List<GameObject> LaserCannons;

    [SerializeField]
    public float LaserSpeed;

    [SerializeField]
    public float LaserDamage;

    [SerializeField]
    public GameObject LaserProjectile;

    [SerializeField]
    public float LaserShotInterval;

    [SerializeField]
    private EnergyBehaviour EnergyBehaviour;

    [SerializeField]
    private float EnergyCostPerShot = 1.0f;

    private bool _shootingLaser;

    private float _lastLaserShot;

    // Start is called before the first frame update
    void Start()
    {
        _lastLaserShot = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootingLaser)
        {
            _attemptShootLasers();
        }
    }

    private void _attemptShootLasers()
    {
        float currentTime = Time.time;
        if ((currentTime - _lastLaserShot) >= LaserShotInterval)
        {
            foreach(GameObject laserCannon in LaserCannons)
            {
                if (EnergyBehaviour == null || EnergyBehaviour.ConsumeEnergy(EnergyCostPerShot) > 0.0f)
                {
                    _shootLaser(laserCannon);
                }
            }
            
            _lastLaserShot = currentTime;
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

    private void _shootLaser(GameObject laserCannon)
    {
        GameObject projectile = Instantiate(LaserProjectile, laserCannon.transform.position, laserCannon.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        SimpleProjectileImpact projectileImpact = projectile.GetComponent<SimpleProjectileImpact>();
        projectileMovement.ProjectileSpeed = LaserSpeed;
        projectileImpact.ProjectileDamage = LaserDamage;
    }

    public override bool IsActive()
    {
        return _shootingLaser;
    }
}
