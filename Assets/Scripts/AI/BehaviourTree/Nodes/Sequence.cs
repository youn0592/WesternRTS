using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

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
