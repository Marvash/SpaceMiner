using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipModShopBhv : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    GameUIManagerSO gameUIManager;
    [SerializeField]
    SpaceshipModShopUI spaceshipModShopUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerLayer = ((1 << collision.gameObject.layer) & playerLayer) > 0;
        if(isPlayerLayer && !collision.isTrigger)
        {
            if(spaceshipModShopUI != null) {
                gameUIManager.ActivateUIWithPriority(spaceshipModShopUI);
            } else {
                Debug.Log("Spaceship mod shop UI is not set");
            }
        }
    }
}
