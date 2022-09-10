using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WorkingTask : Node
{
    bool bFirstCall = true;
    VilliagerAI villager;
    BuildingData buildingData;
    public WorkingTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {
        if(bFirstCall)
        {
            villager.SetAtAWork(true);
            bFirstCall = false;
            return state = NodeState.Running;
        }
        //Again, Hacky Code for temp use.
        buildingData = villager.WorkBuilding.GetComponent<BuildingData>();

        if(VillageBT.gameManager.GetTimeManager().GetDay() >= buildingData.closeTime)
        {
            villager.SetAtAWork(false);
            return state = NodeState.Success;
        }

        return state = NodeState.Running;
    }
}
