using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForEntertainmentTask : Node
{
    VilliagerAI villager;
    GameManager gameManager;

    public CheckForEntertainmentTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
        gameManager = VillageBT.gameManager;
    }

    public override NodeState Eval()
    {
        if (!villager.IsMoraleLow()) return state = NodeState.Failure;

        object t = GetData("Entertainment");

        if (t == null)
        {
            for(int i = 0; i < VillageBT.gameManager.GetBuildingList().Count; i++)
            {
                if (gameManager.GetBuildingList()[i].GetBuildingType() == EBuildings.Saloon
                    && gameManager.GetBuildingList()[i].E_BuildingState == EBuildingState.Open)
                {
                    parent.SetData("Entertainment", gameManager.GetBuildingList()[i].gameObject.transform);
                    return state = NodeState.Success;
                }
            }
            return state = NodeState.Failure;
        }

        return state = NodeState.Success;
    }
}
