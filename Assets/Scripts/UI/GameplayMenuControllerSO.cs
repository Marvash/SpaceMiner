using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameplayMenuControllerSO", menuName = "ScriptableObjects/GameplayMenuControllerSO", order = 1)]
public class GameplayMenuControllerSO : ScriptableObject
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [HideInInspector]
    public UnityEvent OpenPlayershipMenuEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent ClosePlayershipMenuEvent = new UnityEvent();
    [HideInInspector]
    public UnityEvent<PlayershipCargo> OpenMineralShopEvent = new UnityEvent<PlayershipCargo>();
    [HideInInspector]
    public UnityEvent CloseMineralShopEvent = new UnityEvent();

    private bool _playershipMenuOpen = false;

    public void OpenMineralShop(PlayershipCargo cargo)
    {
        if(_playershipMenuOpen)
        {
            TogglePlayershipMenu();
        }
        OpenMineralShopEvent.Invoke(cargo);
        InputDispatcherSO.EnableShopMenuControls();
    }

    public void CloseMineralShop()
    {
        CloseMineralShopEvent.Invoke();
        InputDispatcherSO.EnableBaseGameplayControls();
    }

    public void TogglePlayershipMenu()
    {
        _playershipMenuOpen = !_playershipMenuOpen;
        if(_playershipMenuOpen)
        {
            OpenPlayershipMenu();
        } else
        {
            ClosePlayershipMenu();
        }
    }

    public void OpenPlayershipMenu()
    {
        OpenPlayershipMenuEvent.Invoke();
        InputDispatcherSO.EnablePlayerShipMenuControls();
    }

    public void ClosePlayershipMenu()
    {
        ClosePlayershipMenuEvent.Invoke();
        InputDispatcherSO.EnableBaseGameplayControls();
    }
}
