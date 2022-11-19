using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WorkingTask : Node
{
    bool bFirstCall = true;
    VilliagerAI villager;
    TimeManager timeManager;
    BuildingData buildingData;
    int WorkMonth = 0;
    public WorkingTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
        timeManager = VillageBT.gameManager.GetTimeManager();
    }

    public override NodeState Eval()
    {
        if(bFirstCall && WorkMonth != timeManager.GetMonth())
        {
            villager.SetAtAWork(true);
            bFirstCall = false;
            return state = NodeState.Running;
        }
        //Again, Hacky Code for temp use.
        buildingData = villager.WorkBuilding.GetComponent<BuildingData>();

        if(timeManager.GetDay() >= buildingData.closeTime && WorkMonth != timeManager.GetMonth())
        {
            villager.SetAtAWork(false);
            villager.CollectMorale(-buildingData.GetMoraleCost());
            WorkMonth = timeManager.GetMonth();
            bFirstCall = true;
            return state = NodeState.Success;
        }

        return state = NodeState.Running;
    }
}
