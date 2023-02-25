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
    private PickupCargoSO PickupCargoSO;

    [SerializeField]
    private PlayershipManagerSO PlayershipManagerSO;

    private List<GameObject> _mineralItemList = new List<GameObject>();

    private void Awake()
    {
        _mineralShopCanvas = GetComponent<Canvas>();
        GameplayMenuControllerSO.OpenMineralShopEvent.AddListener(openMineralShopHandler);
        GameplayMenuControllerSO.CloseMineralShopEvent.AddListener(closeMineralShopHandler);
    }

    private void openMineralShopHandler()
    {
        _mineralShopCanvas.enabled = true;
        populateMineralList();
    }

    private void closeMineralShopHandler()
    {
        _mineralShopCanvas.enabled = false;
        clearMineralList();
    }
    
    private void handleItemSold(GameObject soldGO) {
        if (soldGO != null) {
            PickupStack soldPickup = soldGO.GetComponent<MineralItemUI>().GetCurrentPickupStack();
            _mineralItemList.Remove(soldGO);
            Destroy(soldGO);
            PlayershipManagerSO.AddMoney(soldPickup.stackCount * soldPickup.pickupSO.value);
            updateTotalPrice();
            saveCargoChanges();
        }
    }

    private void populateMineralList()
    {
        List<PickupStack> pickups = PickupCargoSO.GetPickups();
        for(int i = 0; i < pickups.Count; i++)
        {
            GameObject mineralItem = Instantiate(MineralItem, MineralListPanel.transform);
            _mineralItemList.Add(mineralItem);
            MineralItemUI mineralItemUI = mineralItem.GetComponent<MineralItemUI>();
            mineralItemUI.SoldSignal.AddListener(handleItemSold);
            mineralItemUI.SetPickupStack(pickups[i]);
        }
        updateTotalPrice();
    }

    private void clearMineralList()
    {
        for (int i = 0; i < _mineralItemList.Count; i++)
        {
            GameObject go = _mineralItemList[i];
            Destroy(go);
        }
        _mineralItemList.Clear();
    }

    private void saveCargoChanges()
    {
        List<PickupStack> psList = new List<PickupStack>();
        for (int i = 0; i < _mineralItemList.Count; i++)
        {
            GameObject go = _mineralItemList[i];
            psList.Add(go.GetComponent<MineralItemUI>().GetCurrentPickupStack());
        }
        PickupCargoSO.SetPickups(psList);
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
        saveCargoChanges();
    }
}
