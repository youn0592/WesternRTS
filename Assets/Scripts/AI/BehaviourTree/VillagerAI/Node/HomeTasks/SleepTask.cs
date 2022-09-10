using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SleepTask : Node
{
    VilliagerAI villager;
    public SleepTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {

        if (VillageBT.gameManager.GetTimeManager().GetDay() <= 8 || VillageBT.gameManager.GetTimeManager().GetDay() >= 20)
        {
            villager.GetComponent<MeshRenderer>().enabled = false;
            return state = NodeState.Running;
        }

        else
        {
            villager.GetComponent<MeshRenderer>().enabled = true;
            return state = NodeState.Success;
        }

        return state = NodeState.Failure;
    }
}
