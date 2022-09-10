using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class EatTask : Node
{
    float i = 0;
    VilliagerAI villager;
    public EatTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {
        i += Time.deltaTime;
        if(i >= 5.0f / VillageBT.gameManager.GetTimeManager().timeMultiplier)
        {
            VillageBT.gameManager.GetTownData().UpdateCurrentFood(-VillageBT.foodAmount);
            villager.CollectFood(VillageBT.foodAmount);
            VillageBT.gameManager.GetUIManager().UpdateFood();
            //VillageBT.bEnabled = true;
            i = 0;
            parent.ClearData("Storage");
            return state = NodeState.Success;
        }

        return state = NodeState.Running;
    }
}
