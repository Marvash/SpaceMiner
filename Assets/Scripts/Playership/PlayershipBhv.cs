using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayershipBhv : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    GameUIManagerSO gameUIManager;

    [SerializeField]
    GameManagerSO gameManager;

    [SerializeField]
    PlayershipUI playershipUI;

    [SerializeField]
    CargoPlayerDataSO playerCargo;

    void Awake() {
        InputDispatcherSO.OpenPlayershipMenu += HandleOpenPlayershipMenu;
        InputDispatcherSO.ClosePlayershipMenu += HandleClosePlayershipMenu;
        gameManager.RegisterPlayer(gameObject);
    }

    void HandleOpenPlayershipMenu() {
        if(playershipUI != null) {
            if(!playershipUI.IsActive) {
                gameUIManager.ActivateUI(playershipUI);
            }
        } else {
            Debug.LogWarning("Playership UI is not set");
        }
    }

    void HandleClosePlayershipMenu() {
        if(playershipUI != null) {
            if(playershipUI.IsActive) {
                gameUIManager.DeactivateUI(playershipUI);
            }
        } else {
            Debug.LogWarning("Playership UI is not set");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == 10)
        {
            PickupStackScript pickupStackScript = other.GetComponent<PickupStackScript>();
            if (pickupStackScript != null && playerCargo.CanAddPickupToCargo(pickupStackScript.pickup))
            {
                playerCargo.AddPickupStackToCargo(pickupStackScript.pickup);
                Destroy(other);
            } else
            {
                Debug.Log("CARGO FULL");
            }
        }
    }
}
