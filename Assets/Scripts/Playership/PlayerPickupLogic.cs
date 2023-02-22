using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupLogic : MonoBehaviour
{
    [SerializeField]
    private PickupCargoSO PickupCargoSO;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == 10)
        {
            PickupStackScript pickupStackScript = other.GetComponent<PickupStackScript>();
            if (pickupStackScript != null && PickupCargoSO.canAddPickupToCargo(pickupStackScript.pickup))
            {
                PickupCargoSO.addPickupStackToInventory(pickupStackScript.pickup);
                Destroy(other);
            } else
            {
                Debug.Log("CARGO FULL");
            }
        }
    }
}