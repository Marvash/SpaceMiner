using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipMenuController : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private GameplayMenuControllerSO GameplayMenuControllerSO;

    private void Awake()
    {
        InputDispatcherSO.ToggleShipMenu += TogglePlayerShipUI;
    }

    public void TogglePlayerShipUI()
    {
        GameplayMenuControllerSO.TogglePlayershipMenu();
    }
}
