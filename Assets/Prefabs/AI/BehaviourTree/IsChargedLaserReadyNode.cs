using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class IsChargedLaserReadyNode : Node
{
    private ChargedLaserCannonArray _chargedLaserCannonArray;

    public IsChargedLaserReadyNode(ChargedLaserCannonArray laserCannonArray)
    {
        _chargedLaserCannonArray = laserCannonArray;
    }
    public override BTState Evaluate()
    {
        if (_chargedLaserCannonArray == null)
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }

        if (_chargedLaserCannonArray.GetChargePercentage() >= 1.0f)
        {
            CurrentState = BTState.SUCCESS;
        } else
        {
            CurrentState = BTState.FAILURE;
        }
        return CurrentState;
    }
}
