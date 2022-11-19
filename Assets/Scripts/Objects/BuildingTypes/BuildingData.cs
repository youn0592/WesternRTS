using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EBuildings
{
    Default = 0,
    GoldCollector,
    Bank,
    Farm,
    WaterWell,
    House,
    Saloon
}
enum ECycleType
{
    Day = 0,
    Month,
    Year
}

public enum EBuildingState
{
    Closed,
    Open
}

public class BuildingData : MonoBehaviour
{

    public string buildingName;

    public int openTime, closeTime;

    [SerializeField]
    int buildingCost, goldPerCycle;

    [SerializeField]
    float moraleCost, crime;

    //Bank Stats
    [HideInInspector, SerializeField]
    int goldIncrease;

    //Farm Stats 
    [HideInInspector, SerializeField]
    int foodIncrease, foodPerCycle, waterCost;

    [HideInInspector, SerializeField]
    int waterIncrease, waterPerCycle;

    [HideInInspector, SerializeField]
    float moraleOffered, crimePerCycle;

    [SerializeField]
    EBuildings E_buildingType;

    [HideInInspector, SerializeField]
    GameObject AIType;
    [HideInInspector]
    public VilliagerAI BuildingAI;

    //Lists for when Multiple employees get added.
    //List<VilliagerAI> m_Employees;
    //List<VilliagerAI> m_Customers;

    event Action ResourceType;

    //TODO - Add a Enum for choosing which Event to call.
    //[SerializeField]
    //ECycleType cycleType;

    public EBuildingState E_BuildingState { get; private set; }

    GameManager m_gameManager;
    TimeManager m_timeManager;
    AIManager m_AIManager;

    bool bIsPlaced = false;

    GameObject m_Lighting;

    private void Start()
    {
        //m_gameManager = GameManager.instance;
        //m_timeManager = m_gameManager.GetTimeManager();

    }

    public void BuildingPlaced()
    {
        m_gameManager = GameManager.instance;
        m_timeManager = m_gameManager.GetTimeManager();
        m_AIManager = m_gameManager.GetAIManager();
        m_Lighting = transform.Find("Lighting").gameObject;
        m_Lighting.SetActive(false);
        E_BuildingState = EBuildingState.Closed;
        bIsPlaced = true;
        SetEvents();
    }

    //Getters
    #region Getters
    public int GetGoldIncrease() { return goldIncrease; }
    public int GetGoldPerCycle() { return goldPerCycle; }
    public int GetGoldCost() { return buildingCost; }
    public float GetMoraleCost() { return moraleCost; }
    public EBuildings GetBuildingType() { return E_buildingType; }
    #endregion

    public void SetEvents()
    {
        if (E_buildingType == EBuildings.Default) return;

        ResourceType += CollectGold;

        switch (E_buildingType)
        {
            case EBuildings.GoldCollector:
                gameObject.tag = "WorkPlace";
                break;
            case EBuildings.Bank:
                m_gameManager.GetTownData().UpdateMaxGold(goldIncrease);
                //gameObject.tag = "WorkPlace";
                break;
            case EBuildings.Farm:
                ResourceType += CollectFood;
                m_gameManager.GetTownData().UpdateMaxFood(foodIncrease);
                gameObject.tag = "WorkPlace";
                break;
            case EBuildings.WaterWell:
                ResourceType += CollectWater;
                m_gameManager.GetTownData().UpdateMaxWater(waterIncrease);
                gameObject.tag = "WorkPlace";
                break;
            case EBuildings.House:
                SpawnAI();
                gameObject.tag = "House";
                E_BuildingState = EBuildingState.Open;
                break;
            case EBuildings.Saloon:
                gameObject.tag = "WorkPlace";
                break;
        }

        m_timeManager.OnDayChanged += ManageLights;
        m_timeManager.OnWeekChanged += CollectResources;

    }

    public void SetOperationState(EBuildingState buildingState)
    {
        if (E_buildingType == EBuildings.House) return;

        this.E_BuildingState = buildingState;
    }

    #region ResourceCollectors  
    public void CollectResources()
    {
        if (E_BuildingState == EBuildingState.Closed) return;
        if (ResourceType != null)
        {
            ResourceType();
        }

        m_gameManager.GetUIManager().UpdateBuildingInfo();
    }
    private void CollectGold()
    {
        //if (E_BuildingState == EBuildingState.Closed) return;
        m_gameManager.GetTownData().UpdateCurrentGold(goldPerCycle);
    }

    private void CollectFood()
    {
        //if (E_BuildingState == EBuildingState.Closed) return;
        m_gameManager.GetTownData().UpdateCurrentFood(foodPerCycle);
    }

    private void CollectWater()
    {
        //if (E_BuildingState == EBuildingState.Closed) return;
        m_gameManager.GetTownData().UpdateCurrentWater(waterPerCycle);
    }

    #endregion

    private void SpawnAI()
    {
        //Transform SP = transform.Find("AISpawn");
        //Transform Spawnpoint = GameObject.Find("AISpawn").transform;
        //AIType = m_AIManager.AIPref.AIPrefab;
        //GameObject newAI = Instantiate(AIType);
        //newAI.transform.position = Spawnpoint.position;
        //newAI.SetActive(true);
        //BuildingAI = newAI.GetComponent<VilliagerAI>();
        m_gameManager.GetAIManager().SpawnAI(transform, this);

    }

    private void ManageLights()
    {
        if (m_Lighting == null)
        {
            Debug.LogError("Lighting was null");
            return;
        }

        if(E_buildingType == EBuildings.Bank)
        {
            Debug.Log(name + ": " + E_BuildingState);
        }

        if (m_timeManager.GetMonth() >= 7 && m_Lighting.activeInHierarchy == false && E_BuildingState == EBuildingState.Open)
        {
            m_Lighting.SetActive(true);
        }
        if (m_timeManager.GetMonth() < 7 && m_Lighting.activeInHierarchy == true || E_BuildingState == EBuildingState.Closed)
        {
            m_Lighting.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (bIsPlaced == true)
        {
            m_timeManager.OnDayChanged -= ManageLights;
            m_timeManager.OnMonthChanged -= CollectGold;
            m_timeManager.OnMonthChanged -= CollectFood;
            m_timeManager.OnMonthChanged -= CollectWater;
            m_timeManager.OnWeekChanged -= CollectResources;
        }
    }


    #region Editor
#if UNITY_EDITOR
    [CustomEditor(typeof(BuildingData))]
    public class BuildingTypesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            BuildingData buildingData = (BuildingData)target;
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Resources are collected 4 times in a month", EditorStyles.boldLabel);

            switch (buildingData.E_buildingType)
            {
                case EBuildings.Default:
                    GUI.color = Color.yellow;
                    EditorGUILayout.LabelField("Default is a null state and won't do anything", EditorStyles.boldLabel);
                    break;
                case EBuildings.Bank:
                    EditorGUILayout.LabelField("Bank Details", EditorStyles.boldLabel);
                    buildingData.goldIncrease = EditorGUILayout.IntField("Gold Increase: ", buildingData.goldIncrease);
                    break;
                case EBuildings.Farm:
                    EditorGUILayout.LabelField("Farm Details", EditorStyles.boldLabel);
                    buildingData.foodIncrease = EditorGUILayout.IntField("Food Increase: ", buildingData.foodIncrease);
                    buildingData.foodPerCycle = EditorGUILayout.IntField("Food Per Cycle: ", buildingData.foodPerCycle);
                    buildingData.waterCost = EditorGUILayout.IntField("Water Cost: ", buildingData.waterCost);
                    break;
                case EBuildings.WaterWell:
                    EditorGUILayout.LabelField("Water Well Details", EditorStyles.boldLabel);
                    buildingData.waterIncrease = EditorGUILayout.IntField("Water Increase: ", buildingData.waterIncrease);
                    buildingData.waterPerCycle = EditorGUILayout.IntField("Water Per Cycle: ", buildingData.waterPerCycle);
                    break;
                case EBuildings.House:
                    EditorGUILayout.LabelField("House Details", EditorStyles.boldLabel);
                    buildingData.AIType = (GameObject)EditorGUILayout.ObjectField("AI Type: ", buildingData.AIType, typeof(GameObject), true);
                    break;
                case EBuildings.Saloon:
                    EditorGUILayout.LabelField("Saloon Details", EditorStyles.boldLabel);
                    buildingData.moraleOffered = EditorGUILayout.FloatField("Morale Offered: ", buildingData.moraleOffered);
                    //buildingData.crimePerCycle = EditorGUILayout.FloatField("Morale Per Cycles: ", buildingData.crimePerCycle);
                    break;
            }


        }

    }
#endif
    #endregion
}
