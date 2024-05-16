using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MineralShopUIController : MonoBehaviour
{
    private Canvas _mineralShopCanvas;

    [SerializeField]
    private GameObject MineralListPanel;

    [SerializeField]
    private GameObject MineralItem;

    [SerializeField]
    private TextMeshProUGUI TotalText;

    [SerializeField]
    private GameplayMenuControllerSO GameplayMenuControllerSO;

    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    private List<GameObject> _mineralItemList = new List<GameObject>();

    private PlayershipCargo _loadedCargo;

    private void Awake()
    {
        _mineralShopCanvas = GetComponent<Canvas>();
        GameplayMenuControllerSO.OpenMineralShopEvent.AddListener(OpenMineralShopHandler);
        GameplayMenuControllerSO.CloseMineralShopEvent.AddListener(CloseMineralShopHandler);
    }

    private void OpenMineralShopHandler(PlayershipCargo cargo)
    {
        _mineralShopCanvas.enabled = true;
        _loadedCargo = cargo;
        PopulateMineralList();
    }

    private void CloseMineralShopHandler()
    {
        _mineralShopCanvas.enabled = false;
        ClearMineralList();
    }
    
    private void HandleItemSold(GameObject soldGO) {
        if (soldGO != null) {
            PickupStack soldPickup = soldGO.GetComponent<MineralItemUI>().GetCurrentPickupStack();
            _mineralItemList.Remove(soldGO);
            Destroy(soldGO);
            PlayershipManagerSO.AddMoney(soldPickup.stackCount * soldPickup.pickupSO.value);
            updateTotalPrice();
            SaveCargoChanges();
        }
    }

    private void PopulateMineralList()
    {
        List<PickupStack> pickups = _loadedCargo.GetPickups();
        for(int i = 0; i < pickups.Count; i++)
        {
            GameObject mineralItem = Instantiate(MineralItem, MineralListPanel.transform);
            _mineralItemList.Add(mineralItem);
            MineralItemUI mineralItemUI = mineralItem.GetComponent<MineralItemUI>();
            mineralItemUI.SoldSignal.AddListener(HandleItemSold);
            mineralItemUI.SetPickupStack(pickups[i]);
        }
        updateTotalPrice();
    }

    private void ClearMineralList()
    {
        for (int i = 0; i < _mineralItemList.Count; i++)
        {
            GameObject go = _mineralItemList[i];
            Destroy(go);
        }
        _mineralItemList.Clear();
    }

    private void SaveCargoChanges()
    {
        List<PickupStack> psList = new List<PickupStack>();
        for (int i = 0; i < _mineralItemList.Count; i++)
        {
            GameObject go = _mineralItemList[i];
            psList.Add(go.GetComponent<MineralItemUI>().GetCurrentPickupStack());
        }
        _loadedCargo.SetPickups(psList);
    }

    public void CloseMineralShopUISignal()
    {
        GameplayMenuControllerSO.CloseMineralShop();
    }

    private void updateTotalPrice()
    {
        int total = 0;
        foreach(GameObject go in _mineralItemList)
        {
            MineralItemUI mineralItemUI = go.GetComponent<MineralItemUI>();
            PickupStack ps = mineralItemUI.GetCurrentPickupStack();
            total += (ps.stackCount * ps.pickupSO.value);
        }
        TotalText.text = total + " $";
    }

    public void SellAllItemsHandler()
    {
        foreach (GameObject go in _mineralItemList)
        {
            PickupStack soldPickup = go.GetComponent<MineralItemUI>().GetCurrentPickupStack();
            Destroy(go);
            PlayershipManagerSO.AddMoney(soldPickup.stackCount * soldPickup.pickupSO.value);
        }
        _mineralItemList.Clear();
        updateTotalPrice();
        SaveCargoChanges();
    }
}
