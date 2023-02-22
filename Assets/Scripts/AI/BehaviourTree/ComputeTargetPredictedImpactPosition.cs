using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;


namespace BehaviourTree
{
    public class ComputeTargetPredictedImpactPosition : Node
    {
        private Vector2 _targetPosition;
        private string _targetPositionBBVarName = "targetPosition";
        public string TargetPositionBBVarName { get => _targetPositionBBVarName; set => _targetPositionBBVarName = value; }
        private Vector2 _targetVelocity;
        private string _targetVelocityBBVarName = "targetVelocity";
        public string TargetVelocityBBVarName { get => _targetVelocityBBVarName; set => _targetVelocityBBVarName = value; }
        private Rigidbody2D _sourceRb;
        private float _projectileSpeed;
        private string _projectileSpeedBBVarName = "projectileSpeed";
        public string ProjectileSpeedBBVarName { get => _projectileSpeedBBVarName; set => _projectileSpeedBBVarName = value; }
        private Vector2 _shooterPosition;

        private string _outputPositionBBVarName = "PredictedImpactPosition";
        public string OutputPositionBBVarName { get => _outputPositionBBVarName; set => _outputPositionBBVarName = value; }

        public ComputeTargetPredictedImpactPosition(GameObject source)
        {
            _sourceRb = source.GetComponent<Rigidbody2D>();
        }
        public override BTState Evaluate()
        {
            object targetPositionBBObj = GetData(_targetPositionBBVarName);
            object targetVelocityBBObj = GetData(_targetVelocityBBVarName);
            object projectileSpeedBBObj = GetData(_projectileSpeedBBVarName);
            if (targetPositionBBObj == null || targetVelocityBBObj == null || projectileSpeedBBObj == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }
            _targetPosition = (Vector2)targetPositionBBObj;
            _targetVelocity = (Vector2)targetVelocityBBObj;
            _projectileSpeed = (float)projectileSpeedBBObj;

            _shooterPosition = _sourceRb.worldCenterOfMass;

            Vector2 finalDirection = Vector2.zero;
            Vector2 finalPosition = Vector2.zero;
            float projectileSpeedPowered = _projectileSpeed * _projectileSpeed;
            float t1, t2;
            float ax = _targetVelocity.x * _targetVelocity.x;
            float bx = (_targetVelocity.x * _targetPosition.x * 2) + (((-_shooterPosition.x) * 2) * _targetVelocity.x);
            float cx = (_targetPosition.x * _targetPosition.x) + (((-_shooterPosition.x) * 2) * _targetPosition.x) + (_shooterPosition.x * _shooterPosition.x);
            float ay = _targetVelocity.y * _targetVelocity.y;
            float by = (_targetVelocity.y * _targetPosition.y * 2) + (((-_shooterPosition.y) * 2) * _targetVelocity.y);
            float cy = (_targetPosition.y * _targetPosition.y) + (((-_shooterPosition.y) * 2) * _targetPosition.y) + (_shooterPosition.y * _shooterPosition.y);
            float a = ax + ay - projectileSpeedPowered;
            float b = bx + by;
            float c = cx + cy;
            //Debug.Log("Trajectory factors: " + a + " " + b + " " + c);

            float delta = (b * b) - (4 * a * c);
            if (delta < 0.0f)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }
            t1 = ((-b) + Mathf.Sqrt(delta)) / (2 * a);
            t2 = ((-b) - Mathf.Sqrt(delta)) / (2 * a);
            //Debug.Log("Trajectory t1 and t2: " + t1 + " " + t2);

            float selectedT = Mathf.Min(t1, t2);
            if (selectedT < 0.0f)
            {
                selectedT = Mathf.Max(t1, t2);
            }
            finalDirection.x = (((_targetVelocity.x * selectedT) + _targetPosition.x) - _shooterPosition.x) / selectedT;
            finalDirection.y = (((_targetVelocity.y * selectedT) + _targetPosition.y) - _shooterPosition.y) / selectedT;
            //Debug.Log("Trajectory result: " + finalDirection);
            finalPosition.x = (finalDirection.x * selectedT) + _shooterPosition.x;
            finalPosition.y = (finalDirection.y * selectedT) + _shooterPosition.y;

            SetDataInRoot(_outputPositionBBVarName, finalPosition);

            CurrentState = BTState.SUCCESS;
            return CurrentState;
        }
    }
}