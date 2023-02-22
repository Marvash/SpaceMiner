using System.Collections.Generic;
using UnityEngine;

public class DropTable : Droppable
{
    private List<Droppable> dropList;
    public DropTable(string ID, uint weight, List<Droppable> dropList) : base(ID, weight)
    {
        this.dropList = dropList;
    }

    public override PickupStack drop()
    {
        uint totalWeight = 0;
        foreach(Droppable droppable in dropList) {
            totalWeight += droppable.weight;
        }
        float randomPick = Random.Range(0.0f, 1.0f);
        float minRandomRange = 0.0f;
        float maxRandomRange = 0.0f;
        foreach (Droppable droppable in dropList)
        {
            minRandomRange = maxRandomRange;
            maxRandomRange = minRandomRange + (droppable.weight / (float)totalWeight);
            if((randomPick >= minRandomRange) && (randomPick <= maxRandomRange))
            {
                return droppable.drop();
            }

        }
        return null;
    }
}
