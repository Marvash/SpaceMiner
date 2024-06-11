using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopBhv : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    GameUIManagerSO gameUIManager;
    [SerializeField]
    WeaponShopUI weaponShopUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerLayer = ((1 << collision.gameObject.layer) & playerLayer) > 0;
        if(isPlayerLayer && !collision.isTrigger)
        {
            if(weaponShopUI != null) {
                gameUIManager.ActivateUIWithPriority(weaponShopUI);
            } else {
                Debug.Log("Weapon shop UI is not set");
            }
        }
    }
}
