using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;


[CreateAssetMenu(fileName = "MoneyPlayerDataSO", menuName = "ScriptableObjects/MoneyPlayerDataSO", order = 1)]
public class MoneyPlayerDataSO : ScriptableObject
{
    private int money;

    [HideInInspector]
    public UnityEvent<int> MoneyUpdateEvent = new UnityEvent<int>();

    private void OnEnable()
    {
        money = 0;
    }

    public void ForceUpdateMoney()
    {
        MoneyUpdateEvent.Invoke(money);
    }

    public void AddMoney(int money)
    {
        this.money += money;
        MoneyUpdateEvent.Invoke(this.money);
    }

    public void SellItem(PickupStack pickupStack) {
        int amount = pickupStack.pickupSO.value * pickupStack.stackCount;
        AddMoney(amount);
    }

    public void SubtractMoney(int money)
    {
        this.money = Mathf.Max(0, this.money + money);
        MoneyUpdateEvent.Invoke(this.money);
    }

    public int GetMoney()
    {
        return money;
    }
}
