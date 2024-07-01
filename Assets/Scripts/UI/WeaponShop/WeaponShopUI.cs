using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponShopUI : MonoBehaviour, IGameUI
{
    [SerializeField]
    private Canvas weaponShopCanvas;
    [SerializeField]
    private Transform weaponShopListContainer;
    [SerializeField]
    private Transform weaponDetailPanelContainer;
    [SerializeField]
    private Transform weaponSlotsContainer;
    [SerializeField]
    private Transform assignToSlotButtonsContainer;
    [SerializeField]
    private WeaponShopLevelNavButtonUI previousWeaponLevelButton;
    [SerializeField]
    private WeaponShopLevelNavButtonUI nextWeaponLevelButton;
    [SerializeField]
    private GameObject weaponShopListItemPrefab;
    [SerializeField]
    private GameObject assignToSlotButtonPrefab;
    [SerializeField]
    private GameObject weaponSlotPrefab;
    [SerializeField]
    TextMeshProUGUI moneyTextField;
    [SerializeField]
    GameObject weaponPurchasePanel;
    [SerializeField]
    GameObject weaponSlotAssignmentPanel;
    [SerializeField]
    GameObject weaponSlotPurchaseButton;
    [SerializeField]
    TextMeshProUGUI weaponSlotPurchasePriceText;

    private List<WeaponShopListItemUI> weaponShopListItems = new List<WeaponShopListItemUI>();

    private WeaponConfigBaseSO[] currentWeaponSlots = new WeaponConfigBaseSO[0];

    private int maxWeaponSlotsCount = 0;
    private int currentWeaponSlotsCount = 0;

    private List<int> weaponSlotPrices = new List<int>();

    private List<WeaponConfigBaseSO> currentWeaponList = new List<WeaponConfigBaseSO>();

    public UnityEvent<IGameUI> OnActivateUI { get; private set;}

    public UnityEvent<IGameUI> OnDeactivateUI { get; private set;}

    public UnityEvent<WeaponConfigBaseSO, int> OnWeaponPurchase { get; private set;}
    public UnityEvent<WeaponConfigBaseSO, int, int> OnWeaponSlotAssign { get; private set;}

    public UnityEvent OnWeaponSlotPurchase { get; private set;}

    public GameInputControls InputControls { get; private set;}

    private WeaponConfigBaseSO detailPanelWeaponConfig = null;

    private int currentWeaponLevelSelected = 0;

    public bool IsActive { get; private set;}
    [field:SerializeField]
    public int Priority { get; set; }

    void Awake()
    {
        OnActivateUI = new UnityEvent<IGameUI>();
        OnDeactivateUI = new UnityEvent<IGameUI>();
        OnWeaponPurchase = new UnityEvent<WeaponConfigBaseSO, int>();
        OnWeaponSlotAssign = new UnityEvent<WeaponConfigBaseSO, int, int>();
        OnWeaponSlotPurchase = new UnityEvent();
        InputControls = GameInputControls.ShopMenu;
        IsActive = false;
    }

    public void InitWeaponShopData(WeaponsPlayerDataSO weaponsPlayerData, int playerCash) {
        detailPanelWeaponConfig = null;
        currentWeaponList = weaponsPlayerData.CompleteWeaponConfigList;
        UpdateWeaponList();
        UpdateWeaponSlots(weaponsPlayerData);
        UpdateBalance(playerCash);
        UpdateWeaponDetailPanel();
    }


    private void UpdateWeaponList() {
        int weaponListItemCount = weaponShopListItems.Count;
        for(int i = weaponListItemCount - 1; i >= 0; i--) {
            weaponShopListItems[i].SetToggleGroup(null);
            weaponShopListItems[i].gameObject.SetActive(false);
            Destroy(weaponShopListItems[i].gameObject);
        }
        weaponShopListItems.Clear();
        ToggleGroup group = weaponShopListContainer.GetComponent<ToggleGroup>();
        foreach(WeaponConfigBaseSO weapon in currentWeaponList) {
            if(weapon.CurrentWeaponLevel > 0) {
                GameObject weaponShopListItem = Instantiate(weaponShopListItemPrefab);
                WeaponShopListItemUI listItem = weaponShopListItem.GetComponent<WeaponShopListItemUI>();
                listItem.UpdateWeaponListItemUI(weapon);
                listItem.SetToggleGroup(group);
                weaponShopListItem.transform.SetParent(weaponShopListContainer, false);
                weaponShopListItems.Add(listItem);
                listItem.OnItemSelect.AddListener(HandleWeaponListItemSelected);
            }
        }
        foreach(WeaponConfigBaseSO weapon in currentWeaponList) {
            if(weapon.CurrentWeaponLevel == 0) {
                GameObject weaponShopListItem = Instantiate(weaponShopListItemPrefab);
                WeaponShopListItemUI listItem = weaponShopListItem.GetComponent<WeaponShopListItemUI>();
                listItem.UpdateWeaponListItemUI(weapon);
                listItem.SetToggleGroup(group);
                weaponShopListItem.transform.SetParent(weaponShopListContainer, false);
                weaponShopListItems.Add(listItem);
                listItem.OnItemSelect.AddListener(HandleWeaponListItemSelected);
            }
        }
    }

    public void UpdateWeaponListItem(WeaponConfigBaseSO weaponConfig) {
        foreach(WeaponShopListItemUI item in weaponShopListItems) {
            if(item.weaponConfig == weaponConfig) {
                item.UpdateWeaponListItemUI(weaponConfig);
            }
        }
    }

    public void UpdateBalance(int playerCash) {
        moneyTextField.text = playerCash + " $";
    }

    public void UpdateWeaponDetailPanel() {
        ResetWeaponDetailPanel();
        if(detailPanelWeaponConfig != null) {
            IWeaponDetailPanelFactory factory = detailPanelWeaponConfig.WeaponDetailPanelFactory;
            AWeaponDetailUI ui = factory.InstantiateWeaponDetailPanel(currentWeaponLevelSelected);
            ui.transform.SetParent(weaponDetailPanelContainer, false);
            if(detailPanelWeaponConfig.CurrentUnlockedWeaponLevel < currentWeaponLevelSelected) {
                if(detailPanelWeaponConfig.CurrentUnlockedWeaponLevel == currentWeaponLevelSelected - 1) {
                    weaponPurchasePanel.SetActive(true);
                } else {
                    weaponPurchasePanel.SetActive(false);
                }
                weaponSlotAssignmentPanel.SetActive(false);
                
            } else {
                weaponPurchasePanel.SetActive(false);
                weaponSlotAssignmentPanel.SetActive(true);
            }
            if(currentWeaponLevelSelected < detailPanelWeaponConfig.WeaponLevels.Count) {
                nextWeaponLevelButton.gameObject.SetActive(true);
                nextWeaponLevelButton.SetButtonLevelText(currentWeaponLevelSelected + 1);
                
            } else {
                nextWeaponLevelButton.gameObject.SetActive(false);
            }
            if(currentWeaponLevelSelected > 1) {
                previousWeaponLevelButton.gameObject.SetActive(true);
                previousWeaponLevelButton.SetButtonLevelText(currentWeaponLevelSelected - 1);
            } else {
                previousWeaponLevelButton.gameObject.SetActive(false);
            }
        }
    }

    private void ResetWeaponDetailPanel() {
        if(weaponDetailPanelContainer.childCount > 0) {
            GameObject currentPanel = weaponDetailPanelContainer.GetChild(0).gameObject;
            currentPanel.SetActive(false);
            Destroy(currentPanel);
        }
        weaponPurchasePanel.SetActive(false);
        weaponSlotAssignmentPanel.SetActive(false);
        nextWeaponLevelButton.gameObject.SetActive(false);
        previousWeaponLevelButton.gameObject.SetActive(false);

    }

    private void HandleWeaponListItemSelected(WeaponConfigBaseSO selectedConfig) {
        if(selectedConfig != detailPanelWeaponConfig) {
            detailPanelWeaponConfig = selectedConfig;
            currentWeaponLevelSelected = detailPanelWeaponConfig.CurrentUnlockedWeaponLevel == 0 ? 1 : detailPanelWeaponConfig.CurrentUnlockedWeaponLevel;
            UpdateWeaponDetailPanel();
        }
    }

    public void UpdateWeaponSlots(WeaponsPlayerDataSO weaponsPlayerData) {
        for(int i = currentWeaponSlotsCount - 1; i >= 0; i--) {
            GameObject weaponSlot = weaponSlotsContainer.transform.GetChild(i).gameObject;
            weaponSlot.SetActive(false);
            Destroy(weaponSlot);
        }
        for(int i = currentWeaponSlotsCount - 1; i >= 0; i--) {
            GameObject weaponAssignSlotBtn = assignToSlotButtonsContainer.transform.GetChild(i).gameObject;
            weaponAssignSlotBtn.SetActive(false);
            Destroy(weaponAssignSlotBtn);
        }
        currentWeaponSlots = weaponsPlayerData.WeaponSlots;
        maxWeaponSlotsCount = weaponsPlayerData.MaxWeaponSlotsCount;
        weaponSlotPrices = weaponsPlayerData.WeaponSlotsPrices;
        currentWeaponSlotsCount = weaponsPlayerData.WeaponSlotsCount;
        for(int i = 0; i < currentWeaponSlotsCount; i++) {
            GameObject weaponSlotGO = Instantiate(weaponSlotPrefab);
            weaponSlotGO.transform.SetParent(weaponSlotsContainer, false);
            weaponSlotGO.transform.SetSiblingIndex(i);
            if(currentWeaponSlots[i] != null) {
                WeaponSlotUI weaponSlot = weaponSlotGO.GetComponent<WeaponSlotUI>();
                weaponSlot.UpdateSlot(currentWeaponSlots[i]);
                weaponSlot.OnWeaponSlotSelected.AddListener(HandleWeaponSlotSelected);
            }
            GameObject assignToSlotGO = Instantiate(assignToSlotButtonPrefab);
            assignToSlotGO.transform.SetParent(assignToSlotButtonsContainer, false);
            AssignToWeaponSlotButtonUI assignToWeaponSlotUI = assignToSlotGO.GetComponent<AssignToWeaponSlotButtonUI>();
            assignToWeaponSlotUI.SetButtonText(i+1);
            assignToWeaponSlotUI.OnAssignToSlot.AddListener(HandleWeaponAssignToSlot);
        }
        if(currentWeaponSlotsCount == maxWeaponSlotsCount) {
            weaponSlotPurchaseButton.SetActive(false);
        } else {
            weaponSlotPurchaseButton.SetActive(true);
            weaponSlotPurchasePriceText.text = weaponSlotPrices[currentWeaponSlotsCount].ToString() + " $";
        }
    }

    private void HandleWeaponAssignToSlot(int slotNumber) {
        OnWeaponSlotAssign.Invoke(detailPanelWeaponConfig, currentWeaponLevelSelected, slotNumber);
    }

    private void HandleWeaponSlotSelected(WeaponConfigBaseSO selectedConfig) {
        if(selectedConfig == detailPanelWeaponConfig) {
            if(currentWeaponLevelSelected != selectedConfig.CurrentWeaponLevel) {
                currentWeaponLevelSelected = detailPanelWeaponConfig.CurrentWeaponLevel == 0 ? 1 : detailPanelWeaponConfig.CurrentWeaponLevel;
                UpdateWeaponDetailPanel();
            }
        } else {
            detailPanelWeaponConfig = selectedConfig;
            currentWeaponLevelSelected = detailPanelWeaponConfig.CurrentWeaponLevel == 0 ? 1 : detailPanelWeaponConfig.CurrentWeaponLevel;
            UpdateWeaponDetailPanel();
        }
        foreach(WeaponShopListItemUI weaponShopListItem in weaponShopListItems) {
            if(weaponShopListItem.weaponConfig == selectedConfig) {
                weaponShopListItem.SetSelectedState(true);
                break;
            }
        }
    }

    public void HandleNextWeaponLevelSelected() {
        currentWeaponLevelSelected++;
        UpdateWeaponDetailPanel();
    }

    public void HandlePreviousWeaponLevelSelected() {
        currentWeaponLevelSelected--;
        UpdateWeaponDetailPanel();
    }

    public void HandleWeaponSlotPurchase() {
        OnWeaponSlotPurchase.Invoke();
    }
    
    public void HandleWeaponPurchase() {
        OnWeaponPurchase.Invoke(detailPanelWeaponConfig, currentWeaponLevelSelected);
    }

    public void ResetUI() {
        int weaponListItemCount = weaponShopListItems.Count;
        for(int i = weaponListItemCount - 1; i >= 0; i--) {
            Destroy(weaponShopListItems[i].gameObject);
        }
        weaponShopListItems.Clear();
        for(int i = currentWeaponSlotsCount - 1; i >= 0; i--) {
            GameObject weaponSlot = weaponSlotsContainer.transform.GetChild(i).gameObject;
            weaponSlot.SetActive(false);
            Destroy(weaponSlot);
        }
        for(int i = currentWeaponSlotsCount - 1; i >= 0; i--) {
            GameObject weaponAssignSlotBtn = assignToSlotButtonsContainer.transform.GetChild(i).gameObject;
            weaponAssignSlotBtn.SetActive(false);
            Destroy(weaponAssignSlotBtn);
        }
        if(detailPanelWeaponConfig != null) {
            GameObject currentPanel = weaponDetailPanelContainer.GetChild(0).gameObject;
            currentPanel.SetActive(false);
            Destroy(currentPanel);
            detailPanelWeaponConfig = null;
        }
        for(int i = currentWeaponSlotsCount - 1; i >= 0; i--) {
            GameObject weaponSlot = weaponSlotsContainer.transform.GetChild(i).gameObject;
            weaponSlot.SetActive(false);
            Destroy(weaponSlot);
        }
        weaponPurchasePanel.SetActive(false);
        weaponSlotAssignmentPanel.SetActive(false);
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
