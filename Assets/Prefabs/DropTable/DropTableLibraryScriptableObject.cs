using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropTableLibraryScriptableObject", menuName = "ScriptableObjects/DropTableLibraryScriptableObject", order = 3)]
public class DropTableLibraryScriptableObject : ScriptableObject
{
    [SerializeField]
    public PickupLibraryScriptableObject pickupLibrarySO;

    Dictionary<string, Droppable> dropTableLibrary;

    public void init()
    {
        dropTableLibrary = new Dictionary<string, Droppable>();
        List<Droppable> asteroidDropList = new List<Droppable>();
        asteroidDropList.Add(new DropPickupStack("IronDrop", 60, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.IRON), 1)));
        asteroidDropList.Add(new DropPickupStack("NickelDrop", 30, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.NICKEL), 1)));
        asteroidDropList.Add(new DropPickupStack("CopperDrop", 10, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.COPPER), 1)));
        dropTableLibrary.Add("AsteroidDropTable", new DropTable("AsteroidDropTable", 0, asteroidDropList));
    }

    public Droppable getDropTable(string dropTableId)
    {
        return dropTableLibrary[dropTableId];
    }
}
