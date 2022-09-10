using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForWaterTask : Node
{
    VilliagerAI villager;
    public CheckForWaterTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {
        if (!villager.IsThirsty()) return state = NodeState.Failure;
        if (!VillageBT.gameManager.GetTownData().IsEnoughWater(VillageBT.waterAmount)) return state = NodeState.Failure;

        object t = GetData("Storage");

        if(t == null)
        {
            for(int i = 0; i < VillageBT.gameManager.GetBuildingList().Count; i++)
            {
                if (VillageBT.gameManager.GetBuildingList()[i].GetBuildingType() == EBuildings.WaterWell)
                {
                    parent.SetData("Storage", VillageBT.gameManager.GetBuildingList()[i].gameObject.transform);
                    return NodeState.Success;
                }
            }

            return state = NodeState.Failure;
        }

        return state = NodeState.Success;
    }
}
