using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopUI : MonoBehaviour
{
    [SerializeField]
    private Canvas weaponShopCanvas;
    [SerializeField]
    private Transform weaponShopListContainer;
    [SerializeField]
    private GameplayMenuControllerSO gameplayMenuControllerSO;

    [SerializeField]
    private GameObject weaponShopListItemPrefab;

    private PlayerWeaponsManager currentWeaponManager;

    private List<GameObject> weaponShopListItems = new List<GameObject>();

    void Awake()
    {
        gameplayMenuControllerSO.OpenWeaponsShopEvent.AddListener(OpenWeaponShopHandler);
        gameplayMenuControllerSO.CloseWeaponsShopEvent.AddListener(CloseWeaponShopHandler);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateWeaponList() {
        Dictionary<int, WeaponConfigBaseSO> ownedWeapons = currentWeaponManager.OwnedWeaponConfigs;
        List<WeaponDescriptorBaseSO> availableWeapons = currentWeaponManager.AvailableWeaponDescriptors;
        foreach(var weapon in ownedWeapons) {
            WeaponShopListItemData weaponListItemData = new WeaponShopListItemData(weapon.Value.WeaponDescriptor, false, weapon.Value.CurrentWeaponLevel);
            GameObject weaponShopListItem = Instantiate(weaponShopListItemPrefab);
            weaponShopListItem.GetComponent<WeaponShopListItemUI>().UpdateWeaponListItemUI(weaponListItemData);
            weaponShopListItem.transform.SetParent(weaponShopListContainer, false);
            weaponShopListItems.Add(weaponShopListItem);
        }
        foreach(WeaponDescriptorBaseSO weaponDescriptor in availableWeapons) {
            if(!ownedWeapons.ContainsKey(weaponDescriptor.WeaponId)) {
                WeaponShopListItemData weaponListItemData = new WeaponShopListItemData(weaponDescriptor, true);
                GameObject weaponShopListItem = Instantiate(weaponShopListItemPrefab);
                weaponShopListItem.GetComponent<WeaponShopListItemUI>().UpdateWeaponListItemUI(weaponListItemData);
                weaponShopListItem.transform.SetParent(weaponShopListContainer, false);
                weaponShopListItems.Add(weaponShopListItem);
            }
        }
    }

    public void OpenWeaponShopHandler(PlayerWeaponsManager weaponManager) {
        weaponShopCanvas.enabled = true;
        currentWeaponManager = weaponManager;
        UpdateUI();
    }

    private void UpdateUI() {
        PopulateWeaponList();
    }

    public void CloseWeaponShopHandler() {
        ResetUI();
        weaponShopCanvas.enabled = false;
    }

    private void ResetUI() {
        foreach(GameObject weaponShopListItem in weaponShopListItems) {
            Destroy(weaponShopListItem);
        }
    }

    public void CloseWeaponShopUISignal()
    {
        gameplayMenuControllerSO.CloseWeaponShop();
    }
}
