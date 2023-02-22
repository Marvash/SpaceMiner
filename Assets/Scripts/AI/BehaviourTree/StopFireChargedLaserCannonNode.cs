using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class StopFireChargedLaserCannonNode : Node
{
    private ChargedLaserCannonArray _chargedLaserCannonArray;

    public StopFireChargedLaserCannonNode(ChargedLaserCannonArray chargedLaserCannonArray)
    {
        _chargedLaserCannonArray = chargedLaserCannonArray;
    }
    public override BTState Evaluate()
    {
        if (_chargedLaserCannonArray == null)
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }

        _chargedLaserCannonArray.ShootEnd();
        CurrentState = BTState.SUCCESS;
        return CurrentState;
    }
}
