using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private PlayershipMenuUIControllerSO PlayershipMenuUIControllerSO;

    private bool _playerShipMenuOpen = false;

    private void Awake()
    {
        InputDispatcherSO.ToggleShipMenu += TogglePlayerShipUI;
    }

    public void TogglePlayerShipUI()
    {
        _playerShipMenuOpen = !_playerShipMenuOpen;

        if (_playerShipMenuOpen)
        {
            PlayershipMenuUIControllerSO.OpenPlayerShipMenu();
            InputDispatcherSO.EnablePlayerShipMenuControls();
        }
        else
        {
            PlayershipMenuUIControllerSO.ClosePlayerShipMenu();
            InputDispatcherSO.EnableBaseGameplayControls();
        }
    }
}
