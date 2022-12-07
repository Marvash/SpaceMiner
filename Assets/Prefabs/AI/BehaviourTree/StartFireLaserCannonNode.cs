using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class StartFireLaserCannonArrayNode : Node
    {
        private LaserCannonArray _laserCannonArray;

        public StartFireLaserCannonArrayNode(LaserCannonArray laserCannonArray)
        {
            _laserCannonArray = laserCannonArray;
        }
        public override BTState Evaluate()
        {
            if (_laserCannonArray == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }

            _laserCannonArray.ShootBegin();
            CurrentState = BTState.SUCCESS;
            return CurrentState;
        }
    }
}