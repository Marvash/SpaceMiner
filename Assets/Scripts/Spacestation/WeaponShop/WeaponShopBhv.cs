using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShopBhv : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayer;

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
        bool isPlayerLayer = ((1 << collision.gameObject.layer) & playerLayer) > 0;
        if(isPlayerLayer && !collision.isTrigger)
        {
            Debug.Log("Player hit");
        }
    }
}
