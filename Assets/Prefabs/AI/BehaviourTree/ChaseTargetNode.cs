using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Pathfinding;

namespace BehaviourTree
{
    public class ChaseTargetNode : Node
    {
        private Rigidbody2D _sourceRb;
        private Vector2 _targetPosition;
        private string _targetPositionBBVarName = "targetPosition";
        public string TargetPositionBBVarName { get => _targetPositionBBVarName; set => _targetPositionBBVarName = value; }
        private Path _path;
        private string _pathBBVarName = "chasePath";
        public string PathBBVarName { get => _pathBBVarName; set => _pathBBVarName = value; }
        private int _currentWaypoint;
        private float _accelerationFactor = 140.0f;
        private string _accelerationFactorBBVarName = "accelerationFactor";
        public string AccelerationFactorBBVarName { get => _accelerationFactorBBVarName; set => _accelerationFactorBBVarName = value; }
        private float _maxSpeed = 1.6f;
        private string _maxSpeedBBVarName = "chaseMaxSpeed";
        public string MaxSpeedBBVarName { get => _maxSpeedBBVarName; set => _maxSpeedBBVarName = value; }
        private float _waypointReachDistance = 1.0f;
        private string _waypointReachDistanceBBVarName = "waypointReachDistance";
        public string WaypointReachDistanceBBVarName { get => _waypointReachDistanceBBVarName; set => _waypointReachDistanceBBVarName = value; }

        public ChaseTargetNode(GameObject source)
        {
            _sourceRb = source.GetComponent<Rigidbody2D>();
        }

        public override BTState Evaluate()
        {
            object targetBBObj = GetData(_targetPositionBBVarName);
            object pathBBObj = GetData(_pathBBVarName);
            object deltaTimeBBObj = GetData("deltaTime");
            if (targetBBObj == null || pathBBObj == null || deltaTimeBBObj == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }
            _targetPosition = (Vector2)targetBBObj;
            float deltaTime = (float)deltaTimeBBObj;
            Path newPath = (Path)pathBBObj;
            if (newPath != _path)
            {
                _path = newPath;
                _currentWaypoint = 0;
            }

            object accelerationFactorBBObj = GetData(_accelerationFactorBBVarName);
            if (accelerationFactorBBObj != null)
            {
                _accelerationFactor = (float)accelerationFactorBBObj;
            }
            object maxSpeedBBObj = GetData(_maxSpeedBBVarName);
            if (maxSpeedBBObj != null)
            {
                _maxSpeed = (float)maxSpeedBBObj;
            }
            object waypointReachDistanceBBObj = GetData(_waypointReachDistanceBBVarName);
            if (waypointReachDistanceBBObj != null)
            {
                _waypointReachDistance = (float)waypointReachDistanceBBObj;
            }

            if (_currentWaypoint >= _path.vectorPath.Count)
            {
                CurrentState = BTState.SUCCESS;
                return CurrentState;
            }
            else
            {
                Vector2 seekDir = ((Vector2)_path.vectorPath[_currentWaypoint] - _sourceRb.worldCenterOfMass).normalized;
                Vector2 seekForce = seekDir * _accelerationFactor * deltaTime;
                _sourceRb.AddForce(seekForce);

                if (_sourceRb.velocity.magnitude > _maxSpeed)
                {
                    _sourceRb.velocity = Vector2.ClampMagnitude(_sourceRb.velocity, _maxSpeed);

                }

                if (_currentWaypoint < (_path.vectorPath.Count - 1))
                {
                    float distanceToNextWaypoint = Vector2.Distance(_sourceRb.worldCenterOfMass, (Vector2)_path.vectorPath[_currentWaypoint]);
                    if (distanceToNextWaypoint < _waypointReachDistance)
                    {
                        _currentWaypoint++;
                    }
                }
            }

            CurrentState = BTState.RUNNING;
            return CurrentState;
        }
    }
}