using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MineralShopUI : MonoBehaviour, IGameUI
{
    private Canvas mineralShopCanvas;

    [SerializeField]
    private GameObject mineralListPanel;

    [SerializeField]
    private GameObject mineralItemPrefab;

    [SerializeField]
    private TextMeshProUGUI totalText;
    [SerializeField]
    private TextMeshProUGUI playerBalanceText;

    private List<MineralItemUI> mineralItemList = new List<MineralItemUI>();

    public UnityEvent<IGameUI> OnActivateUI { get; private set;}

    public UnityEvent<IGameUI> OnDeactivateUI { get; private set;}

    public GameInputControls InputControls { get; private set;}

    private int currentPlayerCash = 999999999;

    public bool IsActive { get; private set;}
    [field:SerializeField]
    public int Priority { get; set; }

    public UnityEvent<int> OnItemSell = new UnityEvent<int>();
    public UnityEvent OnSellAllItems = new UnityEvent();

    private void Awake()
    {
        mineralShopCanvas = GetComponent<Canvas>();
        OnActivateUI = new UnityEvent<IGameUI>();
        OnDeactivateUI = new UnityEvent<IGameUI>();
        InputControls = GameInputControls.ShopMenu;
        IsActive = false;
    }
    
    private void HandleItemSold(MineralItemUI soldItem) {
        if (soldItem != null) {
            OnItemSell.Invoke(soldItem.ItemIndex);
        }
    }

    public void UpdateMineralShop(PickupStack[] cargo, int playerCash)
    {
        ResetUI();
        for(int i = 0; i < cargo.Length; i++)
        {
            if(cargo[i] == null) {
                continue;
            }
            GameObject mineralItemGO = Instantiate(mineralItemPrefab, mineralListPanel.transform);
            MineralItemUI mineralItemUI = mineralItemGO.GetComponent<MineralItemUI>();
            mineralItemList.Add(mineralItemUI);
            mineralItemUI.SoldSignal.AddListener(HandleItemSold);
            mineralItemUI.ItemIndex = i;
            mineralItemUI.SetPickupStack(cargo[i]);
        }
        UpdateTotalPrice();
        currentPlayerCash = playerCash;
        UpdatePlayerBalance();
    }

    private void ResetUI()
    {
        for (int i = 0; i < mineralItemList.Count; i++)
        {
            GameObject go = mineralItemList[i].gameObject;
            Destroy(go);
        }
        mineralItemList.Clear();
    }

    private void UpdateTotalPrice()
    {
        int total = 0;
        foreach(MineralItemUI mineralItemUI in mineralItemList)
        {
            PickupStack ps = mineralItemUI.GetCurrentPickupStack();
            total += ps.stackCount * ps.pickupSO.value;
        }
        totalText.text = total + " $";
    }

    private void UpdatePlayerBalance() {
        playerBalanceText.text = currentPlayerCash + " $";
    }

    public void HandleSellAllItems()
    {
        OnSellAllItems.Invoke();
    }

    public void ActivateUI()
    {
        mineralShopCanvas.enabled = true;
        IsActive = true;
        OnActivateUI.Invoke(this);
    }

    public void DeactivateUI()
    {
        OnDeactivateUI.Invoke(this);
        mineralShopCanvas.enabled = false;
        IsActive = false;
    }
}
