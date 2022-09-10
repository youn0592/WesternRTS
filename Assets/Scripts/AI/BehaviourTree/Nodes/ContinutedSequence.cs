using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    /*Continuted Sequence is different from Sequence as when the first node becomes true
     every node in the sequence is run at the same time.*/
    public class ContinutedSequence : Node
    {
        public ContinutedSequence() : base() { }
        public ContinutedSequence(List<Node> children) : base(children) { }

        public override NodeState Eval()
        {
            bool AnyChildIsRunning = false;
            foreach (Node node in children)
            {
                switch (node.Eval())
                {
                    case NodeState.Failure:
                        return state = NodeState.Failure;
                    case NodeState.Success:
                        AnyChildIsRunning = true;
                        continue;
                    case NodeState.Running:
                        return state = NodeState.Running;
                    default:
                        return state = NodeState.Success;
                }
            }

            return state = AnyChildIsRunning ? NodeState.Running : NodeState.Success;
        }
    }
}
