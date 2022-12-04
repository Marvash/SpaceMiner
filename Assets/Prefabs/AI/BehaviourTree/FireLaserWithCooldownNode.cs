using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class FireLaserWithCooldownNode : Node
    {
        private LaserGun _laserGun;

        private float _laserGunCooldown = 0.5f;
        private string _laserGunCooldownBBVarName = "laserCooldown";
        public string LaserGunCooldownBBVarName { get => _laserGunCooldownBBVarName; set => _laserGunCooldownBBVarName = value; }

        private float _laserGunLastShotTimestamp = 0.0f;

        public FireLaserWithCooldownNode(LaserGun laserGun)
        {
            _laserGun = laserGun;
        }
        public override BTState Evaluate()
        {
            if (_laserGun == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }

            object laserGunCooldownBBObj = GetData(_laserGunCooldownBBVarName);
            if (laserGunCooldownBBObj != null)
            {
                _laserGunCooldown = (float)laserGunCooldownBBObj;
            }

            if ((Time.time - _laserGunLastShotTimestamp) <= _laserGunCooldown) {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }
            _laserGun.ShootLaser();
            _laserGunLastShotTimestamp = Time.time;
            CurrentState = BTState.SUCCESS;
            return CurrentState;
        }
    }
}