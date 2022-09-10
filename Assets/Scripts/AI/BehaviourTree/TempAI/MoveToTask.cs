using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class MoveToTask : Node
{
    private Transform StartLoc;
    private Transform[] waypoints;
    private VilliagerAI villager;

    int waypointIndex;

    float waitTime = 1.0f;
    float waitCounter = 0f;
    bool bWaiting = false;
    public MoveToTask(Transform StartPoint, Transform[] Location, VilliagerAI villagerAI)
    {
        StartLoc = StartPoint;
        waypoints = Location;
        villager = villagerAI;
    }
    public override NodeState Eval()
    {
        if (bWaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
                bWaiting = false;
        }
        else
        {
            Transform wp = waypoints[waypointIndex];
            if (Vector3.Distance(StartLoc.position, wp.position) <= 0.01f)
            {
                StartLoc.position = wp.position;
                waitCounter = 0f;
                bWaiting = true;

                waypointIndex = (waypointIndex + 1) % waypoints.Length;
            }
            else
            {
                StartLoc.position = Vector3.MoveTowards(StartLoc.position, wp.position, (villager.WalkSpeed * VillageBT.gameManager.GetTimeManager().timeMultiplier) * Time.deltaTime);
                StartLoc.LookAt(wp.position);

            }
        }

        return state = NodeState.Running;
    }
}
