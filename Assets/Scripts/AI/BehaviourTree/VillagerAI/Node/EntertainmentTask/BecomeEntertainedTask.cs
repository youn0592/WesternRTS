using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class BecomeEntertainedTask : Node
{
    float timeAtEntertainment;
    float moraleAmount;
    VilliagerAI villager;

    public BecomeEntertainedTask(VilliagerAI villiagerAI)
    {
        villager = villiagerAI;
        moraleAmount = VillageBT.moraleAmount;
    }

    public override NodeState Eval()
    {
        timeAtEntertainment += Time.deltaTime;

        if(timeAtEntertainment > 3 / VillageBT.gameManager.GetTimeManager().timeMultiplier)
        {
            villager.CollectMorale(moraleAmount);
            timeAtEntertainment = 0;
            parent.ClearData("Entertainment");
            return state = NodeState.Success;
        }

        return state = NodeState.Success;
    }
}
