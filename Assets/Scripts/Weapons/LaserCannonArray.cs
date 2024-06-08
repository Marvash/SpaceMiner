using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannonArray : MonoBehaviour, IWeapon
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

    private EnergyBehaviour EnergyBehaviour;

    [SerializeField]
    private float EnergyCostPerShot = 1.0f;

    private bool _shootingLaser;

    private float _lastLaserShot;

    [SerializeField]
    EnergyWeaponConfigSO energyWeaponConfig;

    [SerializeField]
    EnergyWeaponDescriptorSO energyWeaponDescriptor;
    public GameObject PlayershipGO { get; set; }

    public WeaponConfigBaseSO WeaponConfig => energyWeaponConfig;

    void Awake() {
        if(energyWeaponConfig == null) {
            energyWeaponConfig = energyWeaponDescriptor.GetDefaultEnergyWeaponConfig();
        }
    }

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

    public void ShootBegin()
    {
        _shootingLaser = true;
    }

    public void ShootEnd()
    {
        _shootingLaser = false;
    }

    public void ShootInterrupt()
    {
        ShootEnd();
    }

    private void _shootLaser(GameObject laserCannon)
    {
        GameObject projectile = Instantiate(LaserProjectile, laserCannon.transform.position, laserCannon.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        SimpleProjectileDamager projectileImpact = projectile.GetComponent<SimpleProjectileDamager>();
        projectileMovement.ProjectileSpeed = LaserSpeed;
        projectileImpact.ProjectileDamage = LaserDamage;
    }

    public bool IsActive()
    {
        return _shootingLaser;
    }

    public void InitWeapon(WeaponInitializer initializer)
    {
        initializer.InitializeWeapon(this);
    }

    public void SetEnergyBehaviour(EnergyBehaviour energyBehaviour) {
        this.EnergyBehaviour = energyBehaviour;
    }
}
