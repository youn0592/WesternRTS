using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToStorageTask : Node
{
    Transform m_transform;
    Transform savedTransform;
    VilliagerAI villager;
    MeshRenderer VillagerRenderer;

    public GoToStorageTask(Transform transform, VilliagerAI villagerAI, MeshRenderer villageRenderer)
    {
        m_transform = transform;
        savedTransform = transform;
        villager = villagerAI;
        VillagerRenderer = villageRenderer;
    }

    public override NodeState Eval()
    {
        Transform storage = (Transform)GetData("Storage");
        Transform Position = storage.Find("LookAtPoint");

        if(VillagerRenderer.enabled == false)
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
