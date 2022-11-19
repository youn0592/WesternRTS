using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //Managers and Misc objects
    public ObjController objectController;
    public VilliagerAI villagerAI;
    GameManager m_gameManager;
    TimeManager m_timeManager;
    TownData m_townData;
    Test m_gridSystem;

    [SerializeField]
    List<Sprite> moraleSprite = new List<Sprite>();


    //Header UI
    TextMeshProUGUI TownName;
    TextMeshProUGUI GoldUI;
    TextMeshProUGUI FoodUI;
    TextMeshProUGUI WaterUI;
    TextMeshProUGUI YearUI;
    Image moraleImageUI;

    //Building Prompt UI
    GameObject BuildingUIParent;
    TextMeshProUGUI BuildingNameText;
    TextMeshProUGUI BuildingStateText;
    TextMeshProUGUI GoldGainedText;
    TextMeshProUGUI CostText;
    TMP_InputField NameChangePrompt;

    //AI Prompt UI
    GameObject AIUIParent;
    TextMeshProUGUI AINameText;
    TextMeshProUGUI AIAgeText;
    TextMeshProUGUI AIFoodText;
    TextMeshProUGUI AIWaterText;
    TextMeshProUGUI AIMoraleText;
    TMP_Dropdown AIDropdown;

    GameObject HouseButton;

    //Misc UI Objects
    GameObject NamePanel;
    GameObject ChangeNamePanel;
    GameObject NameError;
    GameObject EconomyPanel;
    GameObject DeletePanel;

    GameObject UI_NEGPanel;

    //This Function finds all UI Objects in the hirarchy to have them in code and sets if they need to be active or not
    void Start()
    {
        Instance = this;

        m_gameManager = GameObject.Find("_GAMEMANAGER_").GetComponent<GameManager>();
        m_timeManager = GameObject.Find("_GAMEMANAGER_").GetComponent<TimeManager>();
        m_townData = GameObject.Find("_GAMEMANAGER_").GetComponent<TownData>();
        m_gridSystem = GameObject.Find("Grid").GetComponent<Test>();

        //Building UI Finders
        BuildingUIParent = GameObject.Find("Building UI");
        BuildingNameText = GameObject.Find("Building Name").GetComponent<TextMeshProUGUI>();
        BuildingStateText = GameObject.Find("State Value").GetComponent<TextMeshProUGUI>();
        GoldGainedText = GameObject.Find("GG Value").GetComponent<TextMeshProUGUI>();
        CostText = GameObject.Find("Cost Value").GetComponent<TextMeshProUGUI>();

        //Header UI Finders
        TownName = GameObject.Find("Town Name").GetComponent<TextMeshProUGUI>();
        GoldUI = GameObject.Find("Gold Amount").GetComponent<TextMeshProUGUI>();
        FoodUI = GameObject.Find("Food Amount").GetComponent<TextMeshProUGUI>();
        WaterUI = GameObject.Find("Water Amount").GetComponent<TextMeshProUGUI>();
        YearUI = GameObject.Find("Year UI").GetComponent<TextMeshProUGUI>();
        moraleImageUI = GameObject.Find("Morale Icon").GetComponent<Image>();

        //AI UI Finders
        AIUIParent = GameObject.Find("AI UI");
        AINameText = GameObject.Find("AI Name").GetComponent<TextMeshProUGUI>();
        AIAgeText = GameObject.Find("Age Value").GetComponent<TextMeshProUGUI>();
        AIFoodText = GameObject.Find("Food Value").GetComponent<TextMeshProUGUI>();
        AIWaterText = GameObject.Find("Water Value").GetComponent<TextMeshProUGUI>();
        AIMoraleText = GameObject.Find("Morale Value").GetComponent<TextMeshProUGUI>();
        AIDropdown = GameObject.Find("AI_Building_Dropdown").GetComponent<TMP_Dropdown>();

        //For Testing Purposes
        HouseButton = GameObject.Find("House");

        //Misc UI Finders
        NameChangePrompt = GameObject.Find("ChangeName Prompt").GetComponent<TMP_InputField>();
        NamePanel = GameObject.Find("Name Panel");
        ChangeNamePanel = GameObject.Find("Change Name Panel");
        NameError = GameObject.Find("Name Error");
        UI_NEGPanel = GameObject.Find("NEGPanel");
        EconomyPanel = GameObject.Find("Economy Panel");
        DeletePanel = GameObject.Find("Delete Panel");

        BuildingUIParent.SetActive(false);
        AIUIParent.SetActive(false);
        ChangeNamePanel.SetActive(false);
        NameError.SetActive(false);
        UI_NEGPanel.SetActive(false);
        EconomyPanel.SetActive(false);
        //HouseButton.SetActive(false);

        m_timeManager.OnWeekChanged += UpdateMorale;
    }

    //This function allows the player to change the name of their town.
    public void UpdateTownName()
    {
        if (TownName == null || m_gameManager == null)
        {
            Debug.LogError("Town name or Game Manager were null");
            return;
        }

        TownName.text = m_gameManager.GetTownName();
    }

    //This function updates the UI elements for the Gold system.
    public void UpdateGold()
    {
        if (GoldUI == null || m_townData == null)
        {
            Debug.LogError("Gold UI or TownData Was null");
            return;
        }

        GoldUI.text = ": " + m_townData.GetCurrentGold() + "/" + m_townData.GetMaxGold();

    }

    //This function updates the UI elements for the Day/Month/Year system.
    public void UpdateDMY()
    {
        if (YearUI == null)
        {
            Debug.LogError("Year UI was null");
            return;
        }

        YearUI.text = "D: " + m_gameManager.GetTimeManager().GetDay().ToString("00") + "/" + "M: " + m_gameManager.GetTimeManager().GetMonth().ToString("00") + "/" + "Y: " + m_gameManager.GetTimeManager().GetYear().ToString("00");

    }

    //This function updates the UI elements for the Food system.
    public void UpdateFood()
    {
        if (FoodUI == null || m_townData == null)
        {
            Debug.LogError("FoodUI or TownData was null");
            return;
        }

        FoodUI.text = ": " + m_townData.GetCurrentFood() + "/" + m_townData.GetMaxFood();
    }

    //This function updates the UI elements for the Water system.
    public void UpdateWater()
    {
        if (WaterUI == null || m_townData == null)
        {
            Debug.LogError("WaterUI or TownData were null");
            return;
        }

        WaterUI.text = ": " + m_townData.GetCurrentWater() + "/" + m_townData.GetMaxWater();
    }

    //This function reveals the BuildingUIPanel when a Building has been clicked on.
    public void OnBuildingClicked()
    {
        if (!BuildingUIParent)
        {
            Debug.LogError("BuildingUIParent Was Null");
            return;
        }
        if (BuildingUIParent.activeSelf == false) BuildingUIParent.SetActive(true);

        UpdateBuildingInfo();
    }

    public void UpdateMorale()
    {
        if(m_gameManager == null || moraleImageUI == null)
        {
            Debug.LogError("villagerAI or GameManager or MoraleImage was null");
            return;
        }

        float morale = m_gameManager.GetTownMorale();

        switch(morale)
        {
            case >=65:
                moraleImageUI.sprite = moraleSprite[1];
                break;
            case >=45:
                moraleImageUI.sprite = moraleSprite[2];
                break;
            case >= 0:
                moraleImageUI.sprite = moraleSprite[3];
                break;
            case < 0:
                moraleImageUI.sprite = moraleSprite[0];
                break;
            default:
                moraleImageUI.sprite = moraleSprite[0];
                break;
        }

    }

    //This function updates the UI elements for all the systems in the BuildingUIPanel in case something changes when the panel is open.
    public void UpdateBuildingInfo()
    {
        if (objectController == null) return;
        if (!BuildingNameText || !GoldGainedText || !CostText || !BuildingStateText)
        {
            Debug.LogError("A UI Element for Buildings was null");
            return;
        }


        BuildingNameText.fontSize = 30;
        BuildingNameText.text = objectController.buildingName;
        if (BuildingNameText.text.Length > 14)
        {
            int sizeDeduction = 0;
            for (int i = 0; i < BuildingNameText.text.Length; ++i)
            {
                if (i % 2 == 0)
                {
                    ++sizeDeduction;
                }
            }

            BuildingNameText.fontSize -= sizeDeduction;
        }

        string state = objectController.GetBuildingData().E_BuildingState.ToString();
        string goldGain = objectController.GetBuildingData().GetGoldPerCycle().ToString();
        string cost = objectController.GetBuildingData().GetGoldCost().ToString();

        BuildingStateText.text = state;
        GoldGainedText.text = goldGain;
        CostText.text = cost;
    }

    //This function reveals the AIUIPanel when an AI Villager has been clicked on.
    public void OnAIClicked()
    {
        if (!AIUIParent)
        {
            Debug.LogError("AIUIParent was Null");
            return;
        }
        if (AIUIParent.activeSelf == false) AIUIParent.SetActive(true);

        UpdateAIInfo();
    }

    //This function updates the UI elements for all the systems in the AIUIPanel in case something changes when the panel is open.
    public void UpdateAIInfo()
    {
        if (villagerAI == null) return;
        if (!AINameText || !AIAgeText || !AIFoodText || !AIWaterText)
        {
            Debug.LogError("An AI Element was Null");
            return;
        }

        AINameText.fontSize = 30;
        AINameText.text = villagerAI.AIName;

        string water = villagerAI.GetWater().ToString();
        string food = villagerAI.GetFood().ToString();
        string age = villagerAI.GetVillagerAge().ToString();
        string morale = villagerAI.GetCurrentMorale().ToString();

        AIAgeText.text = age;
        AIWaterText.text = water;
        AIFoodText.text = food;
        AIMoraleText.text = morale;

        UpdateAIDropdownContents();

    }

    //This function updates the workable buildings current open for the AI.
    public void UpdateAIDropdownContents()
    {
        if (!AIDropdown)
        {
            Debug.LogError("AIDropdown was null");
        }

        AIDropdown.ClearOptions();

        List<string> BuildingList = new List<string>();
        BuildingList.Add(villagerAI.WorkBuildingData.buildingName);
        for (int i = 0; i < m_gameManager.GetWorkerlessBuildings().Count; i++)
        {
            BuildingList.Add(m_gameManager.GetWorkerlessBuildings()[i].buildingName);
        }

        AIDropdown.AddOptions(BuildingList);
    }

    //This function closes all panels when the player deslects off of an object.
    public void OnDeselect()
    {
        if (!BuildingUIParent || !AIUIParent)
        {
            Debug.LogError("BuildingParentUI or AIUIParent were null");
            return;
        }

        BuildingUIParent.SetActive(false);
        AIUIParent.SetActive(false);
    }

    //This function changes the panel to allow players to change the name of a selected object.
    public void SelectedNameChangePrompt()
    {
        if (NameChangePrompt == null || BuildingNameText == null)
        {
            Debug.LogError("Name Change prompt or Building Name GUI were null");
            return;
        }

        NameChangePrompt.text = BuildingNameText.text;

    }

    //This function resets everything back to its previous state in case the player doesn't want to change the name of a selected object.
    public void OnNameCancelled()
    {
        if (NameChangePrompt == null || BuildingNameText == null || NameError == null)
        {
            Debug.LogError("Name Change prompt or Building Name GUI were null");
            return;
        }

        NameError.SetActive(false);
        ChangeNamePanel.SetActive(false);
        NamePanel.SetActive(true);
    }

    //This changes the name of the selected object and resets all NameChangePanel settings and resets the objects UI.
    public void OnNameChanged()
    {
        if (NameChangePrompt == null || BuildingNameText == null || NameError == null)
        {
            Debug.LogError("Name Change prompt or Building Name GUI were null");
            return;
        }

        if (NameChangePrompt.text.Length > 29)
        {
            NameError.SetActive(true);
            return;
        }


        objectController.buildingName = NameChangePrompt.text;
        NameError.SetActive(false);
        ChangeNamePanel.SetActive(false);
        NamePanel.SetActive(true);
        OnBuildingClicked();
    }

    //This function is to tell the Grid System which object the Player clicked on so they can place it in the world.
    public void PlaceObject(int index)
    {
        m_gridSystem.SetPlaceableObj(index);
    }

    //This function causes a pop up to appear on the screen if the player doesn't have enough gold to afford the building they want to place.
    public void NotEnoughGold()
    {
        UI_NEGPanel.SetActive(true);
    }

    //This function shows and hides the Extended Economy panel.
    public void SetEconomyPanel()
    {
        EconomyPanel.SetActive(!EconomyPanel.activeInHierarchy);
    }

    //this function tells the GridSystem that the player clicked on the Delete button.
    public void SetDeleteState()
    {
        if(m_gridSystem == null) 
        {
            Debug.LogWarning("Grid System was null");
            return; 
        }

        m_gridSystem.SetDeleteState();
        
    }

    //This function takes in a bool that sets the state of the Delete Panel.
    public void ActivateDeletePanel(bool bPanelState)
    {
        DeletePanel.SetActive(bPanelState);
    }

    //This function is meant to enable the house button a Farm and Water Well has been placed.
    public void EnableHouse(bool bSetHouse)
    {
        //HouseButton.SetActive(bSetHouse);
    }

}
