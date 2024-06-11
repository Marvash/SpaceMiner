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
    private GameObject MineralListPanel;

    [SerializeField]
    private GameObject MineralItem;

    [SerializeField]
    private TextMeshProUGUI TotalText;

    private List<MineralItemUI> mineralItemList = new List<MineralItemUI>();

    public UnityEvent<IGameUI> OnActivateUI { get; private set;}

    public UnityEvent<IGameUI> OnDeactivateUI { get; private set;}

    public GameInputControls InputControls { get; private set;}

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

    public void UpdateMineralList(PickupStack[] cargo)
    {
        ResetUI();
        for(int i = 0; i < cargo.Length; i++)
        {
            if(cargo[i] == null) {
                continue;
            }
            GameObject mineralItem = Instantiate(MineralItem, MineralListPanel.transform);
            MineralItemUI mineralItemUI = mineralItem.GetComponent<MineralItemUI>();
            mineralItemList.Add(mineralItemUI);
            mineralItemUI.SoldSignal.AddListener(HandleItemSold);
            mineralItemUI.ItemIndex = i;
            mineralItemUI.SetPickupStack(cargo[i]);
        }
        UpdateTotalPrice();
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
        TotalText.text = total + " $";
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
