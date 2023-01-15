using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class IsFacingPositionApproximateNode : Node
    {
        private Vector2 _positionToFace;
        private string _positionToFaceBBVarName = "positionToFaceApproximate";
        public string PositionToFaceBBVarName { get => _positionToFaceBBVarName; set => _positionToFaceBBVarName = value; }
        private Rigidbody2D _sourceRb;
        private float _facingToleranceDegrees = 0.06f;
        private string _facingToleranceDegreesBBVarName = "facingToleranceDegrees";
        public string FacingToleranceDegreesBBVarName { get => _facingToleranceDegreesBBVarName; set => _facingToleranceDegreesBBVarName = value; }

        public IsFacingPositionApproximateNode(Transform source)
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

            object facingToleranceDegreesBBObj = GetData(_facingToleranceDegreesBBVarName);
            if (facingToleranceDegreesBBObj != null)
            {
                _facingToleranceDegrees = (float)facingToleranceDegreesBBObj;
            }

            Vector2 faceDirection = (_positionToFace - _sourceRb.worldCenterOfMass).normalized;
            Vector2 currentDirection = new Vector2(Mathf.Cos(_sourceRb.rotation * Mathf.Deg2Rad), Mathf.Sin(_sourceRb.rotation * Mathf.Deg2Rad)).normalized;
            float dp = Vector2.Dot(currentDirection, faceDirection);
            if (dp <= _facingToleranceDegrees)
            {
                CurrentState = BTState.SUCCESS;
            }
            else
            {
                CurrentState = BTState.FAILURE;
            }
            return CurrentState;
        }
    }
}