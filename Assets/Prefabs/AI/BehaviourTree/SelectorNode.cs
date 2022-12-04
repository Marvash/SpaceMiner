using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SelectorNode : Node
    {
        public SelectorNode() : base() { }

        public SelectorNode(List<Node> children) : base(children) { }
        public override BTState Evaluate()
        {
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case BTState.FAILURE:
                        continue;
                    case BTState.SUCCESS:
                        CurrentState = BTState.SUCCESS;
                        return CurrentState;
                    case BTState.RUNNING:
                        CurrentState = BTState.RUNNING;
                        return CurrentState;
                }
            }

            CurrentState = BTState.FAILURE;
            return CurrentState;
        }
    }
}