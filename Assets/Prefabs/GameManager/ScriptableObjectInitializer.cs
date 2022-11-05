using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableObjectInitializer : MonoBehaviour
{
    [SerializeField]
    public PickupLibraryScriptableObject pickupLibrarySO;

    [SerializeField]
    public DropTableLibraryScriptableObject dropTableLibrarySO;

    private void OnEnable()
    {
        pickupLibrarySO.init();
        dropTableLibrarySO.init();
    }
}
