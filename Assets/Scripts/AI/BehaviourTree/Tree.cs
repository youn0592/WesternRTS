using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public abstract class BTree : MonoBehaviour
    {
        private Node root = null;

        protected void Start()
        {
            root = SetupTree();
        }

        private void Update()
        {
            if (root == null) return;

            root.Eval();
        }

        protected abstract Node SetupTree();
    }
}
