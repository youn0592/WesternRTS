using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class DrinkTask : Node
{
    float i;
    int waterAmount;
    VilliagerAI villager;
    public DrinkTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
    }

    public override NodeState Eval()
    {
         i += Time.deltaTime;

        if(i > 3 / VillageBT.gameManager.GetTimeManager().timeMultiplier)
        {
            waterAmount = VillageBT.waterAmount;
            VillageBT.gameManager.GetTownData().UpdateCurrentWater(-waterAmount);
            villager.CollectWater(waterAmount);
            VillageBT.gameManager.GetUIManager().UpdateWater();
            i = 0;
            parent.ClearData("Storage");
            return state = NodeState.Success;
        }
        return state = NodeState.Running;
    }
}
