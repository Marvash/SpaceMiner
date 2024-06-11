using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponShopUI : MonoBehaviour, IGameUI
{
    [SerializeField]
    private Canvas weaponShopCanvas;
    [SerializeField]
    private Transform weaponShopListContainer;

    [SerializeField]
    private GameObject weaponShopListItemPrefab;

    private List<GameObject> weaponShopListItems = new List<GameObject>();

    private WeaponConfigBaseSO[] currentWeaponSlots = new WeaponConfigBaseSO[0];

    private int currentPlayerCash = 999999999;

    private List<WeaponConfigBaseSO> currentWeaponList = new List<WeaponConfigBaseSO>();

    public UnityEvent<IGameUI> OnActivateUI { get; private set;}

    public UnityEvent<IGameUI> OnDeactivateUI { get; private set;}

    public GameInputControls InputControls { get; private set;}

    public bool IsActive { get; private set;}
    [field:SerializeField]
    public int Priority { get; set; }

    void Awake()
    {
        OnActivateUI = new UnityEvent<IGameUI>();
        OnDeactivateUI = new UnityEvent<IGameUI>();
        InputControls = GameInputControls.ShopMenu;
        IsActive = false;
    }

    public void UpdateWeaponShopData(List<WeaponConfigBaseSO> weaponConfigs, WeaponConfigBaseSO[] weaponSlots, int playerCash) {
        ResetUI();
        currentWeaponList = weaponConfigs;
        currentWeaponSlots = weaponSlots;
        currentPlayerCash = playerCash;
        PopulateWeaponList();
    }


    public void PopulateWeaponList() {
        foreach(WeaponConfigBaseSO weapon in currentWeaponList) {
            if(weapon.CurrentWeaponLevel > 0) {
                WeaponShopListItemData weaponListItemData = new WeaponShopListItemData(weapon, false, weapon.CurrentWeaponLevel);
                GameObject weaponShopListItem = Instantiate(weaponShopListItemPrefab);
                weaponShopListItem.GetComponent<WeaponShopListItemUI>().UpdateWeaponListItemUI(weaponListItemData);
                weaponShopListItem.transform.SetParent(weaponShopListContainer, false);
                weaponShopListItems.Add(weaponShopListItem);
            }
        }
        foreach(WeaponConfigBaseSO weapon in currentWeaponList) {
            if(weapon.CurrentWeaponLevel == 0) {
                WeaponShopListItemData weaponListItemData = new WeaponShopListItemData(weapon, true);
                GameObject weaponShopListItem = Instantiate(weaponShopListItemPrefab);
                weaponShopListItem.GetComponent<WeaponShopListItemUI>().UpdateWeaponListItemUI(weaponListItemData);
                weaponShopListItem.transform.SetParent(weaponShopListContainer, false);
                weaponShopListItems.Add(weaponShopListItem);
            }
        }
    }

    private void ResetUI() {
        foreach(GameObject weaponShopListItem in weaponShopListItems) {
            Destroy(weaponShopListItem);
        }
    }

    public void ActivateUI()
    {
        weaponShopCanvas.enabled = true;
        IsActive = true;
        OnActivateUI.Invoke(this);
    }

    public void DeactivateUI()
    {
        OnDeactivateUI.Invoke(this);
        weaponShopCanvas.enabled = false;
        IsActive = false;
    }
}
