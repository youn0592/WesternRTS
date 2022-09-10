using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public static AIManager instance { get; private set; }

    GameManager m_GameManager;
    TimeManager m_TimeManager;

    List<GameObject> m_List;
    List<VilliagerAI> AIList;

    //Temp Code
    //public GameObject AIPref;
    public AIObjectScriptable AIPref;

    public Transform InstanciatePos;

    //Test
    int nameIncrease = 0;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        m_GameManager = GameManager.instance;
        m_TimeManager = m_GameManager.GetTimeManager();
        AIList = new List<VilliagerAI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnAI(Transform buildingTransform, BuildingData buildingData)
    {
        GameObject Go = AIPref.AIPrefab;
        Transform SpawnPoint = buildingTransform.Find("AISpawn");
        Transform WorkTransform = null;
        GameObject NewAI = Instantiate(Go, SpawnPoint.position, Quaternion.identity);
        NewAI.name = "Villager_AI_" + nameIncrease;
        nameIncrease++;
        GameObject AIHolder = GameObject.Find("AIList");
        NewAI.transform.parent = AIHolder.gameObject.transform;
        VilliagerAI SpawningAI = NewAI.GetComponent<VilliagerAI>();
        SpawningAI.AIName = "Mike " + nameIncrease;
        SpawningAI.Birthday = new int[] {m_TimeManager.GetDay(), m_TimeManager.GetMonth(), m_TimeManager.GetYear() };
        //Testing

        AIList.Add(SpawningAI);

        if (SpawningAI == null) { Debug.LogError("SpawningAI was Null"); return; }
        for (int i = 0; i < m_GameManager.GetBuildingList().Count; i++)
        {
            if (m_GameManager.GetBuildingList()[i].tag == "Home" || m_GameManager.GetBuildingList()[i].BuildingAI != null)
                continue;

            m_GameManager.GetBuildingList()[i].BuildingAI = SpawningAI;
            WorkTransform = m_GameManager.GetBuildingList()[i].transform;
            buildingData.BuildingAI = SpawningAI;
            Debug.Log(SpawningAI + " Has a job at " + m_GameManager.GetBuildingList()[i]);
            break;
        }

        SpawningAI.Init(buildingTransform, WorkTransform);
        if(nameIncrease > 1)
        {
            SpawningAI.CollectWater(3);
            SpawningAI.CollectFood(3);
        }
    }

    public void KillAI(VilliagerAI KillableAI)
    {
        AIList.Remove(KillableAI);
    }
}
