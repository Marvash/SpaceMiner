using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralShopBhv : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private GameUIManagerSO gameUIManager;
    [SerializeField]
    private MineralShopUI mineralShopUI;

    [SerializeField]
    LayerMask playerLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerLayer = ((1 << collision.gameObject.layer) & playerLayer) > 0;
        if(isPlayerLayer && !collision.isTrigger)
        {
            if(mineralShopUI != null) {
                gameUIManager.ActivateUIWithPriority(mineralShopUI);
            } else {
                Debug.Log("Mineral shop UI is not set");
            }
        }
    }
}
