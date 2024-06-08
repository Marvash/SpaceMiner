using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargedLaserCannonArray : MonoBehaviour, IWeapon
{
    [SerializeField]
    private List<GameObject> LaserCannons;

    [SerializeField]
    public float LaserMaxSpeed;

    [SerializeField]
    public float LaserMinSpeed;

    [SerializeField]
    public float LaserMaxDamage;

    [SerializeField]
    public float LaserMinDamage;

    [SerializeField]
    public GameObject LaserProjectile;

    [SerializeField]
    public float LaserShotMaxCharge;

    [SerializeField]
    public float LaserShotMinCharge;

    [SerializeField]
    public float LaserShotEnergyAllocationRate = 0.05f;

    private EnergyBehaviour EnergyBehaviour;

    [SerializeField]
    private float EnergyAllocatedPerTick = 0.5f;

    private bool _shootingLaser;

    public bool ConsumesEnergy;

    private bool _charging = false;

    private float _energyAllocated = 0.0f;

    public UnityEvent ChargingStart = new UnityEvent();
    public UnityEvent ChargingStop = new UnityEvent();
    public UnityEvent ChargingComplete = new UnityEvent();
    public UnityEvent LaserShot = new UnityEvent();

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
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootingLaser)
        {
            if(!_charging)
            {
                InvokeRepeating("_allocateEnergy", 0.0f, LaserShotEnergyAllocationRate);
                ChargingStart.Invoke();
                _charging = true;
            }
        } else if(_charging)
        {
            _attemptShootLasers();
            interruptCharge();
        }
    }

    private void _allocateEnergy()
    {
        
        float diff = LaserShotMaxCharge - _energyAllocated;
        if (diff > 0.0f)
        {
            if (ConsumesEnergy)
            {
                _energyAllocated += EnergyBehaviour.AllocateEnergy(Mathf.Min(diff, EnergyAllocatedPerTick));
            } else
            {
                _energyAllocated += Mathf.Min(diff, EnergyAllocatedPerTick);
            }

        }
        else
        {
            ChargingComplete.Invoke();
        }

    }

    private void _attemptShootLasers()
    {
        if (_energyAllocated >= LaserShotMinCharge)
        {
            if (ConsumesEnergy)
            {
                EnergyBehaviour.ConsumeAllocatedEnergy();
            }
            float consumedEnergyPercentage = _energyAllocated / LaserShotMaxCharge;
            foreach (GameObject laserCannon in LaserCannons)
            {
                shootChargedLaser(laserCannon, consumedEnergyPercentage);
            }
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

    public float GetCurrentProjectileSpeed()
    {
        float consumedEnergyPercentage = _energyAllocated / LaserShotMaxCharge;
        return LaserMinSpeed + (LaserMaxSpeed - LaserMinSpeed) * consumedEnergyPercentage;
    }

    private void interruptCharge()
    {
        if (_charging)
        {
            CancelInvoke();
            ChargingStop.Invoke();
            if (ConsumesEnergy)
            {
                EnergyBehaviour.ResetAllocatedEnergy();
            }
            _energyAllocated = 0.0f;
            _charging = false;
        }
    }

    public float GetChargePercentage()
    {
        return _energyAllocated / LaserShotMaxCharge;
    }

    private void shootChargedLaser(GameObject laserCannon, float chargePercentage)
    {
        GameObject projectile = Instantiate(LaserProjectile, laserCannon.transform.position, laserCannon.transform.rotation);
        SimpleProjectileMovement projectileMovement = projectile.GetComponent<SimpleProjectileMovement>();
        SimpleProjectileDamager projectileImpact = projectile.GetComponent<SimpleProjectileDamager>();
        projectileMovement.ProjectileSpeed = LaserMinSpeed + (LaserMaxSpeed - LaserMinSpeed) * chargePercentage;
        projectileImpact.ProjectileDamage = LaserMinDamage + (LaserMaxDamage - LaserMinDamage) * chargePercentage;
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
