using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToTargetTask : Node
{
    Transform transform;
   public GoToTargetTask(Transform newTransform)
    {
        transform = newTransform;
    }

    public override NodeState Eval()
    {
        Transform target = (Transform)GetData("target");

        if (Vector3.Distance(transform.position, target.position) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, VilliagerBT.speed * Time.deltaTime);
            transform.LookAt(target.position);
        }
        else
        {
            target.GetComponent<BoxCollider>().enabled = false;
            ClearData("target");
            return state = NodeState.Success;
        }

        return state = NodeState.Running;
    }
}
