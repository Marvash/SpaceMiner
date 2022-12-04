using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class HasLOSOnTargetPositionNode : Node
    {
        private Vector2 _targetPosition;
        private string _targetPositionBBVarName = "LOSTargetPosition";
        public string TargetPositionBBVarName { get => _targetPositionBBVarName; set => _targetPositionBBVarName = value; }
        private Rigidbody2D _sourceRb;
        private int _raycastObstacleMask = 1 << 8;
        private string _raycastObstacleMaskBBVarName = "LOSRaycastObstacleMask";
        public string RaycastObstacleMaskBBVarName { get => _raycastObstacleMaskBBVarName; set => _raycastObstacleMaskBBVarName = value; }

        public HasLOSOnTargetPositionNode(GameObject source)
        {
            _sourceRb = source.GetComponent<Rigidbody2D>();
        }
        public override BTState Evaluate()
        {
            object targetPositionBBObj = (Vector2)GetData(_targetPositionBBVarName);
            if (targetPositionBBObj == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }

            _targetPosition = (Vector2)targetPositionBBObj;

            object raycastObstacleMaskBBObj = GetData(_raycastObstacleMaskBBVarName);
            if (raycastObstacleMaskBBObj != null)
            {
                _raycastObstacleMask = (int)raycastObstacleMaskBBObj;
            }

            Vector2 origin = _sourceRb.position;
            Vector2 rayDir = (_targetPosition - origin).normalized;
            float rayLength = (_targetPosition - origin).magnitude;
            RaycastHit2D rayHit = Physics2D.Raycast(origin, rayDir, rayLength, _raycastObstacleMask);
            if (rayHit.collider == null)
            {
                Debug.Log("HAS LOS");

                CurrentState = BTState.SUCCESS;
                return CurrentState;
            }
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }
    }
}