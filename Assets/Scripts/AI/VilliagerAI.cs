using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VilliagerAI : MonoBehaviour
{
    public enum EVilliagerAge
    {
        Child = 0,
        Adult,
        Elder,
        Dead
    }

    enum EAIState
    {
        AtHome,
        Working
    }

    EVilliagerAge E_VillagerAge;
    EAIState E_AIState;

    TimeManager m_TimeManager { get; set; }
    UIManager m_UIManager;

    public float WalkSpeed;

    public string AIName;
    [SerializeField]
    int CurrentFood;
    [SerializeField]
    int MaxFood;
    [SerializeField]
    int CurrentWater;
    [SerializeField]
    int MaxWater;


    public int[] Birthday; //0 - is day, 1 - is month, 2 - is year.
    private bool bIsBirthMonth = false;

    public Transform HomeBuilding { get; private set; }
    public Transform WorkBuilding { get; private set; }
    public BuildingData WorkBuildingData { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        if (m_TimeManager == null) m_TimeManager = TimeManager.Instance;
        if (m_UIManager == null) m_UIManager = UIManager.Instance;
    }

    public void Init(Transform HomeTransform, Transform WorkTransform)
    {
        m_TimeManager = TimeManager.Instance;
        m_UIManager = UIManager.Instance;
        E_VillagerAge = EVilliagerAge.Child;
        HomeBuilding = HomeTransform;
        WorkBuilding = WorkTransform;
        WorkBuildingData = WorkBuilding.GetComponent<BuildingData>();

        m_TimeManager.OnMonthChanged += IsAIBirthday;
        m_TimeManager.OnMonthChanged += DrinkWater;
        m_TimeManager.OnMonthChanged += EatFood;

        if(Birthday[1] == 1)
        {
            Birthday[1] = 13;
        }

        if (CurrentFood == 0 || CurrentWater == 0)
        {
            Debug.Log("A Spawning AI has no Food or Water, and is going to die immediately");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentWater <= 0 || CurrentFood <= 0)
        {
            KillVillager();
        }
    }
    public void SetAtAWork(bool bIsAtWork)
    {
        BuildingData workData = WorkBuilding.GetComponent<BuildingData>();

        if (bIsAtWork)
        {
            E_AIState = EAIState.Working;
            workData.SetOperationState(EBuildingState.Open);
            m_UIManager.UpdateBuildingInfo();
            return;
        }

        workData.SetOperationState(EBuildingState.Closed);
        workData.CollectResources();
        E_AIState = EAIState.AtHome;
    }

    void IsAIBirthday()
    {
        if (m_TimeManager.GetMonth() == Birthday[1] && bIsBirthMonth == false)
        {
            m_TimeManager.OnDayChanged += IsAIBirthday;
            bIsBirthMonth = true;
        }

        if (m_TimeManager.GetDay() == Birthday[0] && bIsBirthMonth == true)
        {
            AgeAI();
            m_TimeManager.OnDayChanged -= IsAIBirthday;
        }

        if (m_TimeManager.GetMonth() != Birthday[1])
        {
            bIsBirthMonth = false;
        }
    }
    void AgeAI()
    {
        E_VillagerAge++;

        switch (E_VillagerAge)
        {
            case EVilliagerAge.Child:
                WalkSpeed = 10;
                break;
            case EVilliagerAge.Adult:
                WalkSpeed = 20;
                break;
            case EVilliagerAge.Elder:
                WalkSpeed = 5;
                break;
        }
        Debug.Log(E_VillagerAge);
        //m_UIManager.OnAIClicked();
    }

    private void EatFood()
    {
        CurrentFood--;
        m_UIManager.UpdateAIInfo();
    }

    private void DrinkWater()
    {
        CurrentWater--;
        m_UIManager.UpdateAIInfo();
    }

    public void CollectFood(int FoodAmount)
    {
        CurrentFood += FoodAmount;

        if (CurrentFood >= MaxFood)
        {
            CurrentFood = MaxFood;
        }
        m_UIManager.UpdateAIInfo();
    }

    public void CollectWater(int WaterAmount)
    {
        CurrentWater += WaterAmount;

        if (CurrentWater >= MaxWater)
        {
            CurrentWater = MaxWater;
        }
        m_UIManager.UpdateAIInfo();
    }

    public bool IsHungry()
    {
        if (CurrentFood <= 2)
        {
            return true;
        }

        return false;
    }

    public bool IsThirsty()
    {
        if (CurrentWater <= 2)
        {
            return true;
        }

        return false;
    }

    //UI Getters
    public int GetFood() { return CurrentFood; }
    public int GetWater() { return CurrentWater; }
    public EVilliagerAge GetVillagerAge() { return E_VillagerAge; }

    public void KillVillager()
    {
        //Temp code - TODO -   Add an actual death / killing state 
        E_VillagerAge = EVilliagerAge.Dead;
        GameManager.instance.GetAIManager().KillAI(this);
        m_TimeManager.OnMonthChanged -= DrinkWater;
        m_TimeManager.OnMonthChanged -= EatFood;
        m_TimeManager.OnMonthChanged -= IsAIBirthday;
        m_TimeManager.OnDayChanged -= IsAIBirthday;
        Destroy(gameObject);
    }
}
