using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {

        protected Node _root = null;

        void Start()
        {
            _root = SetupTree();
            InitTree();
        }

        protected void tick(float deltaTime)
        {
            if(_root != null)
            {
                _root.SetData("deltaTime", deltaTime);
                UpdateBBVariables();
                _root.Evaluate();
            }
        }

        protected abstract Node SetupTree();
        protected abstract void UpdateBBVariables();
        protected abstract void InitTree();

    }
}