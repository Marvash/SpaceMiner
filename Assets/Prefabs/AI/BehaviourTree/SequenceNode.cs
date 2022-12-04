using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SequenceNode : Node
    {
        public SequenceNode() : base() { }

        public SequenceNode(List<Node> children) : base(children) { }
        public override BTState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach(Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case BTState.FAILURE:
                        CurrentState = BTState.FAILURE;
                        return CurrentState;
                    case BTState.SUCCESS:
                        continue;
                    case BTState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                }
            }

            if (anyChildIsRunning)
            {
                CurrentState = BTState.RUNNING;
            }
            else
            {
                CurrentState = BTState.SUCCESS;
            }
            return CurrentState;
        }
    }
}