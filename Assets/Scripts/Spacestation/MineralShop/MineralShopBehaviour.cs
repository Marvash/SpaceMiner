using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralShopBehaviour : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private GameplayMenuControllerSO GameplayMenuControllerSO;

    [SerializeField]
    private int playerLayer;

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
        if(collision.gameObject.layer == playerLayer && !collision.isTrigger)
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
