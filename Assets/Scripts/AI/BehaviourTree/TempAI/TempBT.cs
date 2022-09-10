using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using BehaviourTree;
public class VilliagerBT : BTree
{    
    public Transform[] waypoints;
    
    public static float speed = 10f;
    public static float FovRange = 6f;

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node> { new CheckEnemyTask(transform), new GoToTargetTask(transform)}),
            //new MoveToTask(transform, waypoints)
        }) ;
        return root;
    }
}
