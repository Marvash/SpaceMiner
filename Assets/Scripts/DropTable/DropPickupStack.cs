using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DropPickupStack : Droppable
{
    private PickupStack pickupDrop;
    public DropPickupStack(string ID, uint weight, PickupStack pickupDrop) : base(ID, weight)
    {
        this.pickupDrop = pickupDrop;
    }

    public override PickupStack drop()
    {
        return pickupDrop.clone();
    }
}
