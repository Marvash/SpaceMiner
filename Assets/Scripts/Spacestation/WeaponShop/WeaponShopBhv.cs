using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopBhv : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    GameplayMenuControllerSO gameplayMenuController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerLayer = ((1 << collision.gameObject.layer) & playerLayer) > 0;
        if(isPlayerLayer && !collision.isTrigger)
        {
            PlayerWeaponsManager weapons = collision.GetComponent<PlayerWeaponsManager>();
            gameplayMenuController.OpenWeaponShop(weapons);
        }
    }
}
