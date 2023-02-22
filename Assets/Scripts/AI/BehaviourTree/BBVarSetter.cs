using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class BBVarSetter : Node
    {
        private string _varName;
        private object _varValue;

        public BBVarSetter(string varName, object varValue)
        {
            _varName = varName;
            _varValue = varValue;
        }

        public override BTState Evaluate()
        {
            SetDataInRoot(_varName, _varValue);
            CurrentState = BTState.SUCCESS;
            return CurrentState;
        }
    }
}