using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ResultInverterNode : Node
    {
        public ResultInverterNode(Node child) : base(new List<Node>() { child }) { }
        public override BTState Evaluate()
        {
            foreach (Node node in Children)
            {
                switch (node.Evaluate())
                {
                    case BTState.FAILURE:
                        CurrentState = BTState.SUCCESS;
                        return CurrentState;
                    case BTState.SUCCESS:
                        CurrentState = BTState.FAILURE;
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
