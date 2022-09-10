using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForFoodTask : Node
{
    VilliagerAI villager;
    public CheckForFoodTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {
        if (!villager.IsHungry()) return state = NodeState.Failure;
        if(!VillageBT.gameManager.GetTownData().IsEnoughFood(VillageBT.foodAmount)) return state = NodeState.Failure;
        object t = GetData("Storage");

        if (t == null)
        {
            for(int i = 0; i < VillageBT.gameManager.GetBuildingList().Count; i++)
            {
                if(VillageBT.gameManager.GetBuildingList()[i].GetBuildingType() == EBuildings.Farm)
                {
                    parent.SetData("Storage", VillageBT.gameManager.GetBuildingList()[i].gameObject.transform);
                    return state = NodeState.Success;
                }
            }

            return state = NodeState.Failure;
        }

        return state = NodeState.Success;
    }
}
