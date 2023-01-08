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
        dropTableLibrary.Add("CommonAsteroidDropTable", new DropTable("CommonAsteroidDropTable", 0, asteroidDropList));
        asteroidDropList = new List<Droppable>();
        asteroidDropList.Add(new DropPickupStack("CopperDrop", 30, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.COPPER), 1)));
        asteroidDropList.Add(new DropPickupStack("NickelDrop", 50, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.NICKEL), 1)));
        asteroidDropList.Add(new DropPickupStack("GoldDrop", 10, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.GOLD), 1)));
        dropTableLibrary.Add("UncommonAsteroidDropTable", new DropTable("UncommonAsteroidDropTable", 0, asteroidDropList));
        asteroidDropList = new List<Droppable>();
        asteroidDropList.Add(new DropPickupStack("CopperDrop", 60, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.COPPER), 1)));
        asteroidDropList.Add(new DropPickupStack("GoldDrop", 25, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.GOLD), 1)));
        asteroidDropList.Add(new DropPickupStack("PlatinumDrop", 10, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.PLATINUM), 1)));
        asteroidDropList.Add(new DropPickupStack("DiamondDrop", 5, new PickupStack(pickupLibrarySO.getPickupSO(PickupScriptableObject.PickupId.DIAMOND), 1)));
        dropTableLibrary.Add("RareAsteroidDropTable", new DropTable("RareAsteroidDropTable", 0, asteroidDropList));
    }

    public Droppable getDropTable(string dropTableId)
    {
        return dropTableLibrary[dropTableId];
    }
}
