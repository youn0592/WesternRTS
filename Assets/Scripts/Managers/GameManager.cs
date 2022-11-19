using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public enum GameState
    {
        Default = 0,
        Building,
        Managing,
        Paused
    }

    GameState currentState = GameState.Default;

    public event Action OnBuildingPlaced;
    public event Action OnBuildingDestroyed;

    UIManager m_UIManager;
    TimeManager m_TimeManager;
    TownData m_TownData;
    AIManager m_AIManager;
    
    List<BuildingData> m_BuildingList;

    float m_townMorale;

    private void Start()
    {
        instance = this;
        m_BuildingList = new List<BuildingData>();
        m_UIManager = GameObject.Find("_UIMANAGER_").GetComponent<UIManager>();
        m_TimeManager = GetComponent<TimeManager>();
        m_TownData = GetComponent<TownData>();
        m_AIManager = GetComponent<AIManager>();
        if (m_UIManager == null)
        {
            Debug.LogError("UI Manager was null");
        }
        if(m_TimeManager == null)
        {
            Debug.LogError("Time Manager was null");
        }
        if(m_TownData == null)
        {
            Debug.LogError("Town Data was NULL");
        }

        Init();
    }

    private void Init()
    {
        m_UIManager.UpdateTownName();
        m_UIManager.UpdateGold();
        m_UIManager.UpdateFood();
        m_UIManager.UpdateWater();
        m_UIManager.UpdateDMY();
    }

    private void Update()
    {
        m_UIManager.UpdateDMY();
    }

    public void AddBuildingToList(BuildingData newBuilding)
    {
        if(newBuilding == null) return;

        newBuilding.BuildingPlaced();
        m_BuildingList.Add(newBuilding);

        if(OnBuildingPlaced != null)
        {
            OnBuildingPlaced();
        }
        //CheckForBuildings();
    }

    public void RemoveBuildingFromList(BuildingData buildingToBeRemoved)
    {
        m_BuildingList.Remove(buildingToBeRemoved);

        if(OnBuildingDestroyed != null)
        {
            OnBuildingDestroyed();
        }
        //CheckForBuildings();
    }

    void CheckForBuildings()
    {
        int temp = 0;
        for(int i = 0; i < m_BuildingList.Count; i++)
        {
            if(m_BuildingList[i].GetBuildingType() == EBuildings.WaterWell || m_BuildingList[i].GetBuildingType() == EBuildings.Farm)
            {
                temp++;
            }
        }

        if(temp >= 2)
        {
            m_UIManager.EnableHouse(true);
        }
        else
        {
            m_UIManager.EnableHouse(false);
        }
    }

    public List<BuildingData> GetWorkerlessBuildings()
    {
        List<BuildingData> workerlessBuildings = new List<BuildingData>();

        for(int i = 0; i < m_BuildingList.Count; i++)
        {
            if (!m_BuildingList[i].BuildingAI)
            {
                workerlessBuildings.Add(m_BuildingList[i]);
            }
        }

        return workerlessBuildings;
    }

    public void UpdateGoldUI()
    {
        if(m_UIManager == null)
        {
            Debug.LogError("m_UIManager was null in UpdateGoldUI()");
            return;
        }

        m_UIManager.UpdateGold();
    }

    public float GetTownMorale()
    {
        if(m_AIManager == null || m_UIManager == null)
        {
            Debug.LogError("AI Manager or UI Manager were null");
            return -999.0f;
        }
        m_townMorale = 0;

        if(m_AIManager.GetVillagerList().Count == 0)
        {
            return -999.0f;
        }

        for(int i = 0; i < m_AIManager.GetVillagerList().Count; i++)
        {
            m_townMorale += m_AIManager.GetVillagerList()[i].GetCurrentMorale();
        }
        m_townMorale /= m_AIManager.GetVillagerList().Count;

        if(m_townMorale <= 0.0f)
        {
            m_townMorale = 0.0f;
        }
        else if(m_townMorale >= 100.0f)
        {
            m_townMorale = 100.0f;
        }

        return m_townMorale;
    }

    public string GetTownName(){ return m_TownData.GetTownName(); }

    public TownData GetTownData(){ return m_TownData; }

    public void SetGameState(GameState newState)
    {
        currentState = newState;
    }

    public GameState GetGameState(){ return currentState; }

    public List<BuildingData> GetBuildingList(){ return m_BuildingList; }
    public bool IsBuildingInList(EBuildings BuildingType)
    {
        for(int i = 0; i < m_BuildingList.Count; i++)
        {
            if(BuildingType == m_BuildingList[i].GetBuildingType())
            {
                return true;
            }
        }
        return false;
    }
    public TimeManager GetTimeManager(){ return m_TimeManager; }
    public UIManager GetUIManager(){ return m_UIManager; }
    public AIManager GetAIManager(){ return m_AIManager; }
}
