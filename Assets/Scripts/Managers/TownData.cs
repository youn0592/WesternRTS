using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownData : MonoBehaviour
{
    GameManager m_GameManager;

    [SerializeField]
    string m_TownName;

    //Gold Fields
    [SerializeField]
    int m_CurrentGoldAmount, m_MaxGoldAmount;

    //Food Fields
    [SerializeField]
    int m_CurrentFoodAmount, m_MaxFoodAmount;

    //Water Fields
    [SerializeField]
    int m_CurrentWaterAmount, m_MaxWaterAmount;

    //Morale and Crime Fields
    [SerializeField]
    float m_CurrentMorale, m_CurrentCrime;

    void Start()
    {
        m_GameManager = GetComponent<GameManager>();

        if (m_GameManager == null)
        {
            Debug.LogError("GameManager was null");
            return;
        }

    }

    void Update()
    {
    }

    #region TownName Functions
    public void SetTownName(string newName)
    {
        m_TownName = newName;
    }    
    public string GetTownName(){ return m_TownName; }

    #endregion

    public float UpdateCurrentMorale()
    {
        if(m_GameManager == null)
        {
            Debug.LogError("GameManager was null");
            return 0;
        }

        List<VilliagerAI> Villagers = m_GameManager.GetAIManager().GetVillagerList();

        m_CurrentMorale = 0.0f;

        for(int i = 0; i < Villagers.Count; i++)
        {
            m_CurrentMorale += Villagers[i].GetCurrentMorale();
        }

        m_CurrentMorale /= Villagers.Count;

        Debug.Log(m_CurrentMorale);
        return m_CurrentMorale;
    }
    public float UpdateCurrentCrime()
    {
        if(m_GameManager == null)
        {
            Debug.LogError("Game Manager was null");
            return 0;
        }

        List<VilliagerAI> Villagers = m_GameManager.GetAIManager().GetVillagerList();
        m_CurrentCrime = 0.0f;

        for(int i = 0; i < Villagers.Count; i++)
        {
            m_CurrentCrime += Villagers[i].GetCurrentCrime();
        }

        m_CurrentCrime /= Villagers.Count;

        return m_CurrentCrime;
    }

    #region Gold Functions
    public void UpdateCurrentGold(int amount)
    {
        m_CurrentGoldAmount += amount;

        if (m_CurrentGoldAmount >= m_MaxGoldAmount)
        {
            m_CurrentGoldAmount = m_MaxGoldAmount;
        }

        m_GameManager.UpdateGoldUI();
    }
    public void UpdateMaxGold(int amount)
    {
        m_MaxGoldAmount += amount;
        m_GameManager.UpdateGoldUI();
    }
    #endregion

    #region Food Functions
    public void UpdateCurrentFood(int amount)
    {
        m_CurrentFoodAmount += amount;
        if(m_CurrentFoodAmount >= m_MaxFoodAmount)
        {
            m_CurrentFoodAmount = m_MaxFoodAmount;
        }

        GameManager.instance.GetUIManager().UpdateFood();
        //TODO - Add UI Update
    }    

    public void UpdateMaxFood(int amount)
    {
        m_MaxFoodAmount += amount;
        GameManager.instance.GetUIManager().UpdateFood();
        //TODO - Add UI Update
    }

    public bool IsEnoughFood(int amount)
    {
        if(amount > m_CurrentFoodAmount)
        {
            return false;
        }

        return true;
    }    
    #endregion

    #region Water Functions
    public void UpdateCurrentWater(int amount)
    {
        m_CurrentWaterAmount += amount;
        if(m_CurrentWaterAmount >= m_MaxWaterAmount)
        {
            m_CurrentWaterAmount = m_MaxWaterAmount;
        }
        GameManager.instance.GetUIManager().UpdateWater();
        //TODO - Add UI Update.
    }

    public void UpdateMaxWater(int amount)
    {
        m_MaxWaterAmount += amount;
        GameManager.instance.GetUIManager().UpdateWater();
        //TODO - Add UI update
    }

    public bool IsEnoughWater(int amount)
    {
        if (amount > m_CurrentWaterAmount)
            return false;

        return true;
    }
    #endregion

    #region Getters

    public int  GetCurrentGold(){ return m_CurrentGoldAmount; }
    public int GetMaxGold(){ return m_MaxGoldAmount; }

    public int GetCurrentFood() {  return m_CurrentFoodAmount; }
    public int GetMaxFood() {  return m_MaxFoodAmount; }

    public int GetCurrentWater() { return m_CurrentWaterAmount; }
    public int GetMaxWater() { return m_MaxWaterAmount; }

    #endregion
}
