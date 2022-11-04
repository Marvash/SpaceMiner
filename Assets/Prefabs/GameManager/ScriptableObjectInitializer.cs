using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectInitializer : MonoBehaviour
{
    [SerializeField]
    public PickupLibraryScriptableObject pickupLibrarySO;

    [SerializeField]
    public DropTableLibraryScriptableObject dropTableLibrarySO;
    private void Start()
    {
        pickupLibrarySO.init();
        dropTableLibrarySO.init();
    }
}
