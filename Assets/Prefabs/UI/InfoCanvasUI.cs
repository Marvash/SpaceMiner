using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCanvasUI : MonoBehaviour
{
    [SerializeField]
    PickupInventoryScriptableObject pickupInventorySO;

    private Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
        pickupInventorySO.infoWindowOpenCloseEvent.AddListener(handleOpenCloseInfoWindow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void handleOpenCloseInfoWindow(bool open)
    {
        canvas.enabled = open;
    }
}
