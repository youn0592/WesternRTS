using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class DrinkTask : Node
{
    float timeAtStorage;
    int waterAmount;
    VilliagerAI villager;
    public DrinkTask(VilliagerAI villagerAI)
    {
        villager = villagerAI;
        waterAmount = VillageBT.waterAmount;
    }

    public override NodeState Eval()
    {
        timeAtStorage += Time.deltaTime;

        if(timeAtStorage > 3 / VillageBT.gameManager.GetTimeManager().timeMultiplier)
        {
            VillageBT.gameManager.GetTownData().UpdateCurrentWater(-waterAmount);
            villager.CollectWater(waterAmount);
            VillageBT.gameManager.GetUIManager().UpdateWater();
            timeAtStorage = 0;
            parent.ClearData("Storage");
            return state = NodeState.Success;
        }
        return state = NodeState.Running;
    }
}
