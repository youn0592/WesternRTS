using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForHomeTask : Node
{
    VilliagerAI villager;
    public CheckForHomeTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }
    public override NodeState Eval()
    {
        if (!villager.HomeBuilding) return state = NodeState.Failure;

        object t = GetData("HomeBuilding");

        if(t == null)
        {
            parent.SetData("HomeBuilding", villager.HomeBuilding);
            return state = NodeState.Success;
        }

        return NodeState.Success;
    }
}
