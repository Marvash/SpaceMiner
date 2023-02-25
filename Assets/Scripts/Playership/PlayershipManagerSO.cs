using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;


[CreateAssetMenu(fileName = "PlayershipManagerSO", menuName = "ScriptableObjects/PlayershipManagerSO", order = 1)]
public class PlayershipManagerSO : ScriptableObject
{
    [HideInInspector]
    public GameObject Player;

    private int _money;

    [HideInInspector]
    public UnityEvent<int> MoneyUpdateEvent = new UnityEvent<int>();

    private void OnEnable()
    {
        _money = 0;
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player; 
    }

    public void ForceUpdateMoney()
    {
        MoneyUpdateEvent.Invoke(_money);
    }

    public void AddMoney(int money)
    {
        _money += money;
        MoneyUpdateEvent.Invoke(_money);
    }

    public void SubtractMoney(int money)
    {
        _money = Mathf.Max(0, _money + money);
        MoneyUpdateEvent.Invoke(_money);
    }

    public int GetMoney()
    {
        return _money;
    }
}
