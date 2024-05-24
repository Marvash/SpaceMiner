using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralShopBhv : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private GameplayMenuControllerSO GameplayMenuControllerSO;

    [SerializeField]
    LayerMask playerLayer;

    private List<PickupStack> _sellableMinerals;

    private void Awake()
    {
        InputDispatcherSO.CloseShopMenu += CloseMineralShop;
    }

    public void SellMinerals(PlayershipCargo cargo)
    {
        GameplayMenuControllerSO.OpenMineralShop(cargo);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isPlayerLayer = ((1 << collision.gameObject.layer) & playerLayer) > 0;
        if(isPlayerLayer && !collision.isTrigger)
        {
            PlayershipCargo cargo = collision.GetComponent<PlayershipCargo>();
            SellMinerals(cargo);
        }
    }
    
    private void CloseMineralShop()
    {
        GameplayMenuControllerSO.CloseMineralShop();
    }
}
