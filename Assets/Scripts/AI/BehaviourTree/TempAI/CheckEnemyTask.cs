using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
public class CheckEnemyTask : Node
{
    private static int enemyLayer = 1 << 7;
    Transform transform;

    public CheckEnemyTask(Transform enemy)
    {
        transform = enemy; 
    }

    public override NodeState Eval()
    {
        object t = GetData("target");
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, TempBT.FovRange, enemyLayer);

            if(colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                return state = NodeState.Success;
            }

            return state = NodeState.Failure;
        }

        return state = NodeState.Success;
    }
}
