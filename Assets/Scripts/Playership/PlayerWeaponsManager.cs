using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInitializer {
    GameObject playerShipGO;
    public WeaponInitializer(GameObject playerShipGO) {
        this.playerShipGO = playerShipGO;
    }
    public void InitializeWeapon(LaserCannonArray laserCannonArray) {
        EnergyBehaviour energyBehaviour = playerShipGO.GetComponent<EnergyBehaviour>();
        laserCannonArray.SetEnergyBehaviour(energyBehaviour);
    }

    public void InitializeWeapon(ChargedLaserCannonArray chargedLaserCannonArray) {
        EnergyBehaviour energyBehaviour = playerShipGO.GetComponent<EnergyBehaviour>();
        chargedLaserCannonArray.SetEnergyBehaviour(energyBehaviour);
    }
}

public class PlayerWeaponsManager : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private GameObject WeaponsGO;

    public List<IWeapon> weapons {get; private set;}

    private int weaponSlotsCount = 4;

    private int selectedWeaponIndex;

    private bool shooting;

    private WeaponInitializer weaponInitializer;

    [field:SerializeField]
    public List<WeaponDescriptorBaseSO> AvailableWeaponDescriptors {get; set;}

    public Dictionary<int, WeaponConfigBaseSO> OwnedWeaponConfigs {get; private set;}

    private void Start()
    {
        weaponInitializer = new WeaponInitializer(gameObject);
        OwnedWeaponConfigs = new Dictionary<int, WeaponConfigBaseSO>();
        weapons = new List<IWeapon>();
        selectedWeaponIndex = 0;
        InputDispatcherSO.SelectWeaponSlot += selectWeaponHandler;
        InputDispatcherSO.FirePrimaryStart += shootStartHandler;
        InputDispatcherSO.FirePrimaryStop += shootStopHandler;
        InputDispatcherSO.IncDecWeaponSlot += incDecWeaponHandler;
        int weaponCount = WeaponsGO.transform.childCount;
        for(int i = 0; i < weaponCount && weapons.Count < weaponSlotsCount; i++) {
            RegisterWeapon(WeaponsGO.transform.GetChild(i).gameObject);
        }
    }

    public bool RegisterWeapon(GameObject weaponGO) {
        if(weaponGO.TryGetComponent(out IWeapon weapon)) {
            if(weapons.Contains(weapon)) {
                return false;
            }
            if(weaponGO.transform.parent != WeaponsGO) {
                weaponGO.transform.SetParent(WeaponsGO.transform, false);
            }
            weapon.InitWeapon(weaponInitializer);
            WeaponConfigBaseSO weaponConfig = weapon.WeaponConfig;
            if(!AvailableWeaponDescriptors.Contains(weaponConfig.WeaponDescriptor)) {
                AvailableWeaponDescriptors.Add(weaponConfig.WeaponDescriptor);
            }
            OwnedWeaponConfigs.Add(weaponConfig.WeaponDescriptor.WeaponId, weaponConfig);
            Debug.Log("Found weapon " + weaponConfig.WeaponDescriptor.WeaponName);
            weapons.Add(weapon);
            return true;
        } else {
            return false;
        }
    }

    private void shootStartHandler()
    {
        weapons[selectedWeaponIndex]?.ShootBegin();
        shooting = true;
    }

    private void shootStopHandler()
    {
        weapons[selectedWeaponIndex]?.ShootEnd();
        shooting = false;
    }

    private void selectWeaponHandler(int slot)
    {
        SwapWeapon(slot - 1);
    }

    private void incDecWeaponHandler(float value)
    {
        if(value > 0.0f)
        {
            increaseWeaponIndex();
        } else if(value < 0.0f)
        {
            decreaseWeaponIndex();
        }
    }

    private void increaseWeaponIndex()
    {
        int newIndex = (selectedWeaponIndex + 1) % weapons.Count;
        SwapWeapon(newIndex);
    }

    private void decreaseWeaponIndex()
    {
        int newIndex = (selectedWeaponIndex - 1) % weapons.Count;
        if(newIndex < 0)
        {
            newIndex = weapons.Count - 1;
        }
        SwapWeapon(newIndex);
    }

    private void SwapWeapon(int newWeaponIndex)
    {
        if (newWeaponIndex % weapons.Count != selectedWeaponIndex)
        {
            weapons[selectedWeaponIndex]?.ShootInterrupt();
            selectedWeaponIndex = newWeaponIndex;
            if (shooting)
            {
                weapons[selectedWeaponIndex]?.ShootBegin();
            }
        }
    }

}
