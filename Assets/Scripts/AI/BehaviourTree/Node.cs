using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviourTree
{
    public class Node
    {
        public enum BTState
        {
            SUCCESS,
            RUNNING,
            FAILURE
        }

        public Node Parent;
        protected BTState CurrentState;
        protected List<Node> Children = new List<Node>();
        private Dictionary<string, object> _blackBoard = new Dictionary<string, object>();

        public Node()
        {
            Parent = null;
        }

        public Node(List<Node> children)
        {
            Parent = null;
            foreach (Node child in children)
            {
                AddChild(child);
            }
        }

        private void AddChild(Node child)
        {
            child.Parent = this;
            Children.Add(child);
        }

        public virtual BTState Evaluate()
        {
            CurrentState = BTState.FAILURE;
            return CurrentState;
        }

        public void SetData(string key, object value)
        {
            _blackBoard[key] = value;
        }

        public void SetDataInRoot(string key, object value)
        {
            if(Parent != null)
            {
                Parent.SetDataInRoot(key, value);
            } else
            {
                SetData(key, value);
            }
        }

        public object GetData(string key)
        {
            object value = null;
            if(_blackBoard.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = Parent;
            while(node != null)
            {
                value = node.GetData(key);
                if(value != null)
                {
                    return value;
                }
                node = node.Parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (_blackBoard.ContainsKey(key))
            {
                _blackBoard.Remove(key);
                return true;
            }

            Node node = Parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.Parent;
            }

            return false;
        }
    }
}
