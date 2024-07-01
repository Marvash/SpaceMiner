using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;


[CreateAssetMenu(fileName = "MoneyPlayerDataSO", menuName = "ScriptableObjects/MoneyPlayerDataSO", order = 1)]
public class MoneyPlayerDataSO : ScriptableObject
{
    [SerializeField]
    private int money;

    [HideInInspector]
    public UnityEvent<int> MoneyUpdateEvent = new UnityEvent<int>();

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

    public bool TrySubtractMoney(int money)
    {
        if(this.money >= money) {
            this.money -= money;
            MoneyUpdateEvent.Invoke(this.money);
            return true;
        } else {
            return false;
        }
    }

    public int GetCurrentBalance()
    {
        return money;
    }
}
