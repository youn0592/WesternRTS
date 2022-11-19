using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class VillageBT : BTree
{
    public Transform[] waypoints;

    public static GameManager gameManager;
    public VilliagerAI villager;
    public MeshRenderer VillagerRenderer;

    public static int foodAmount = 5;
    public static int waterAmount = 5;
    public static float moraleAmount = 20.0f; //Temp code, should move to the Saloon.
    public static bool bEnabled = false;

    protected override Node SetupTree()
    {
        gameManager = GameObject.Find("_GAMEMANAGER_").GetComponent<GameManager>();
        villager = GetComponent<VilliagerAI>();
        VillagerRenderer = villager.GetComponent<MeshRenderer>();

        Node node = new Selector(new List<Node>
        {
            //Sequence for Checking and Getting Water
            new Sequence(new List<Node> { new CheckForWaterTask(villager), new GoToStorageTask(transform, villager, VillagerRenderer), new DrinkTask(villager) }),
            //Sequence for Checking and Getting Food.
            new Sequence(new List<Node> { new CheckForFoodTask(villager), new GoToStorageTask(transform, villager, VillagerRenderer), new EatTask(villager) }),
            //Task for going to work
            new Sequence(new List<Node> { new CheckForWorkTask(villager), new GoToWorkTask(transform, villager, VillagerRenderer), new WorkingTask(villager)}),
            //Task for finding Entertainment
            new Sequence(new List<Node> {new CheckForEntertainmentTask(villager), new GoToEntertainmentTask(transform, villager, VillagerRenderer), new BecomeEntertainedTask(villager)}),
            //Task for Going Home
            new Sequence(new List<Node> { new CheckForHomeTask(villager), new GoToHomeTask(transform, villager, VillagerRenderer), new SleepTask(villager)})
            //Task for patroling.
            //new MoveToTask(transform, waypoints, villager)
            });

        return node;
    }

}
