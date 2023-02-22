using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class TargetInRangeNode : Node
    {
        private float _attackRange = 1.0f;
        private string _attackRangeBBVarName = "attackRange";
        public string AttackRangeBBVarName { get => _attackRangeBBVarName; set => _attackRangeBBVarName = value; }
        private Vector2 _target;
        private string _targetBBVarName = "attackRangeTarget";
        public string TargetBBVarName { get => _targetBBVarName; set => _targetBBVarName = value; }

        private GameObject _source;

        public TargetInRangeNode(GameObject source)
        {
            _source = source;
        }

        public override BTState Evaluate()
        {
            object targetBBObj = GetData(_targetBBVarName);
            if (targetBBObj == null)
            {
                CurrentState = BTState.FAILURE;
                return CurrentState;
            }
            _target = (Vector2)targetBBObj;
            object attackRangeBBObj = GetData(_attackRangeBBVarName);
            if (attackRangeBBObj != null)
            {
                _attackRange = (float)attackRangeBBObj;
            }

            float attackRangeSquared = _attackRange * _attackRange;
            if ((_target - (Vector2)_source.transform.position).sqrMagnitude <= attackRangeSquared)
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