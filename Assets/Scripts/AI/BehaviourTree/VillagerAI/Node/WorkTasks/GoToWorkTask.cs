using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToWorkTask : Node
{
    Transform m_transform;
    VilliagerAI villager;
    MeshRenderer VillagerRenderer;

    public GoToWorkTask(Transform transform, VilliagerAI villagerAI, MeshRenderer villagerRender)
    {
        m_transform = transform;
        villager = villagerAI;
        VillagerRenderer = villagerRender;
    }

    public override NodeState Eval()
    {
        Transform WorkPlace = (Transform)GetData("WorkPlace");
        Transform Position = WorkPlace.Find("LookAtPoint");

        if (VillagerRenderer.enabled == false)
        {
            VillagerRenderer.enabled = true;
        }

        if (Vector3.Distance(m_transform.position, Position.position) >= 1)
        {
            m_transform.position = Vector3.MoveTowards(m_transform.position, Position.position, villager.WalkSpeed * VillageBT.gameManager.GetTimeManager().timeMultiplier * Time.deltaTime);
            m_transform.LookAt(Position);
            return state = NodeState.Running;
        }
        else
        {
            return state = NodeState.Success;
        }
    }
}
