using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableObjectInitializer : MonoBehaviour
{
    [SerializeField]
    private PickupLibrarySO PickupLibrarySO;

    [SerializeField]
    private DropTableLibrarySO DropTableLibrarySO;

    private void OnEnable()
    {
        PickupLibrarySO.init();
        DropTableLibrarySO.init();
    }
}
