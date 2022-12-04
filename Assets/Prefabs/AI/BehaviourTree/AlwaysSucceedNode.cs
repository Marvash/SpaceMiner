using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class AlwaysSucceedNode : Node
    {
        public AlwaysSucceedNode(Node child) : base(new List<Node>() { child }) { }
        public override BTState Evaluate()
        {
            foreach (Node node in Children)
            {
                node.Evaluate();
            }

            CurrentState = BTState.SUCCESS;
            return CurrentState;
        }
    }
}