using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForWorkTask : Node
{
    VilliagerAI villager;
    BuildingData buildingData;
    public CheckForWorkTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {
        if (!villager.WorkBuilding) return state = NodeState.Failure;
        //Hacky Code, should make BuildingData getable in villager.
        buildingData = villager.WorkBuilding.GetComponent<BuildingData>();
        if(!buildingData) return state = NodeState.Failure;
        if (VillageBT.gameManager.GetTimeManager().GetDay() < buildingData.openTime
            || VillageBT.gameManager.GetTimeManager().GetDay() > buildingData.closeTime) return state = NodeState.Failure;
        object t = GetData("WorkPlace");

        if (t == null || !IsCurrentWorkplace())
        {
            parent.SetData("WorkPlace", villager.WorkBuilding);
            return state = NodeState.Success;
        }

        return state = NodeState.Success;
    }

    bool IsCurrentWorkplace()
    {
        if(parent.GetData("WorkPlace").Equals(villager.WorkBuilding))
        {
            return true;
        }

        return false;
    }
}
