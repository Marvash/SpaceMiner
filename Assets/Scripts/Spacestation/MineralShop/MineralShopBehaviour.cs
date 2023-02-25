using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralShopBehaviour : MonoBehaviour
{
    [SerializeField]
    private InputDispatcherSO InputDispatcherSO;

    [SerializeField]
    private PickupCargoSO PickupCargoSO;

    [SerializeField]
    private GameplayMenuControllerSO GameplayMenuControllerSO;

    [SerializeField]
    private int playerLayer;

    private List<PickupStack> _sellableMinerals;

    private void Awake()
    {
        InputDispatcherSO.CloseShopMenu += closeMineralShop;
    }

    public void SellMinerals(List<PickupStack> minerals)
    {
        Debug.Log("Selling minerals " + minerals.Count);
        if (minerals.Count > 0)
        {
            _sellableMinerals = minerals;
            foreach (PickupStack ps in _sellableMinerals)
            {
                Debug.Log("Mineral: x" + ps.stackCount + " " + ps.pickupSO.name);
            }
        }
        GameplayMenuControllerSO.OpenMineralShop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == playerLayer && !collision.isTrigger)
        {
            SellMinerals(PickupCargoSO.GetPickups());
        }
    }
    
    private void closeMineralShop()
    {
        GameplayMenuControllerSO.CloseMineralShop();
    }
}
