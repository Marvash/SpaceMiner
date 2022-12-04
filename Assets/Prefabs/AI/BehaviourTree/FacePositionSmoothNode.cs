using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class FacePositionSmoothNode : Node
    {
        private Vector2 _positionToFace;
        private string _positionToFaceBBVarName = "positionToFace";
        public string PositionToFaceBBVarName { get => _positionToFaceBBVarName; set => _positionToFaceBBVarName = value; }
        private Rigidbody2D _sourceRb;
        private float _rotSmoothness = 0.05f;
        private string _rotSmoothnessBBVarName = "rotSmoothness";
        public string RotSmoothnessBBVarName { get => _rotSmoothnessBBVarName; set => _rotSmoothnessBBVarName = value; }
        private float _rotVelocity;

        public FacePositionSmoothNode(GameObject source)
        {
            _sourceRb = source.GetComponent<Rigidbody2D>();
        }

        public override BTState Evaluate()
        {
            object positionToFaceBBObj = GetData(_positionToFaceBBVarName);
            if (positionToFaceBBObj == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }
            _positionToFace = (Vector2)positionToFaceBBObj;

            object rotSmoothnessBBObj = GetData("RotSmoothnessFactor");
            if (rotSmoothnessBBObj != null)
            {
                _rotSmoothness = (float)rotSmoothnessBBObj;
            }

            Vector2 faceDirection = (_positionToFace - _sourceRb.worldCenterOfMass).normalized;

            float targetAngle = (Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg) - 90.0f;
            float currentAngle = _sourceRb.rotation;
            float nextAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref _rotVelocity, _rotSmoothness);

            _sourceRb.MoveRotation(nextAngle);

            CurrentState = BTState.RUNNING;
            return CurrentState;
        }
    }
}