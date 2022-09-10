using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState Eval()
        {
            foreach (Node node in children)
            {
                switch (node.Eval())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        return state = NodeState.Success;
                    case NodeState.Running:
                        return state = NodeState.Running;
                    default:
                        continue;
                }
            }

            return state = NodeState.Failure;
        }
    }
}
