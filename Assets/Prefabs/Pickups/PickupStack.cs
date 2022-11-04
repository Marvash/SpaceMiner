using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class PickupStack
{
    public int stackCount;
    public PickupScriptableObject pickupSO;

    public PickupStack(PickupScriptableObject pickupSO, int stackCount)
    {
        this.pickupSO = pickupSO;
        this.stackCount = stackCount;
    }

    public PickupStack clone()
    {
        return new PickupStack(pickupSO, stackCount);
    }


}