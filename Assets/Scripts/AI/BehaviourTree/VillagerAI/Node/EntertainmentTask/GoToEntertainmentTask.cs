using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToEntertainmentTask : Node
{
    Transform m_Transform;
    VilliagerAI m_Villager;
    MeshRenderer VillagerRenderer;

    public GoToEntertainmentTask(Transform transform, VilliagerAI villagerAI, MeshRenderer villageRenderer)
    {
        m_Transform = transform;
        m_Villager = villagerAI;
        VillagerRenderer = villageRenderer;
    }

    public override NodeState Eval()
    {
        Transform Entertainment = (Transform)GetData("Entertainment");
        Transform LookAtPoint = Entertainment.Find("LookAtPoint");

        if (VillagerRenderer.enabled == false)
        {
            VillagerRenderer.enabled = true;
        }

        if (Vector3.Distance(m_Transform.position, LookAtPoint.position) >= 1)
        {
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, LookAtPoint.position, m_Villager.WalkSpeed * VillageBT.gameManager.GetTimeManager().timeMultiplier * Time.deltaTime);
            m_Transform.LookAt(LookAtPoint.position);
            return state = NodeState.Running;
        }
        else 
        {
            return state = NodeState.Success;
        }


    }
}
