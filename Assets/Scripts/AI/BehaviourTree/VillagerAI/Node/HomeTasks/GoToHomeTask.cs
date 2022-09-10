using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToHomeTask : Node
{
    Transform m_Transform;
    VilliagerAI villager;
    MeshRenderer VillagerRenderer;
    public GoToHomeTask(Transform transform, VilliagerAI villagerAI, MeshRenderer VillagerAIRenderer)
    {
        m_Transform = transform;
        villager = villagerAI;
        VillagerRenderer = VillagerAIRenderer;
    }

    public override NodeState Eval()
    {
        Transform Home = (Transform)GetData("HomeBuilding");
        Transform LookAtPoint = Home.Find("LookAtPoint");

        if (VillagerRenderer.enabled == false)
        {
            VillagerRenderer.enabled = true;
        }

        if (Vector3.Distance(m_Transform.position, LookAtPoint.position) >= 1)
        {
            m_Transform.position = Vector3.MoveTowards(m_Transform.position, LookAtPoint.position, villager.WalkSpeed * VillageBT.gameManager.GetTimeManager().timeMultiplier * Time.deltaTime);
            m_Transform.LookAt(LookAtPoint);
            return state = NodeState.Running;
        }
        else
        {
            return state = NodeState.Success;
        }
    }
}
