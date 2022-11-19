using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class ObjController : MonoBehaviour
{

    public static ObjController Create(Vector3 worldPosition, Vector2Int origin, BuildingObjectScriptable.Dir dir, BuildingObjectScriptable SOobject)
    {


        Transform objectTransform = Instantiate(SOobject.prefab, worldPosition, Quaternion.Euler(0, SOobject.GetRotationAngle(dir), 0));
        ObjController placedObj = objectTransform.GetComponent<ObjController>();

        if (placedObj == null)
        {
            Debug.LogError("Building Object does not have ObjController attached");
            return null;
        }

        placedObj.placedObjSO = SOobject;
        placedObj.dir = dir;
        placedObj.orgin = origin;

        return placedObj;
    }

    private BuildingObjectScriptable placedObjSO;
    private Vector2Int orgin;
    private BuildingObjectScriptable.Dir dir;
    private bool bEnoughGold;


    public string buildingName;
    public int currentResidents;
    public int maxResidents;
    public int currentHealth;
    public int maxHealth;
    //public int goldCost;
    //public int goldPerMonth;
    //public int goldIncrease;

    BuildingData buildingData;

    void Start()
    {
        buildingData = gameObject.GetComponent<BuildingData>();
    }

    public int GetGoldCost() 
    {
        buildingData = gameObject.GetComponent<BuildingData>();
        return buildingData.GetGoldCost(); 
    }

    //public void CollectGold()
    //{
    //    GameManager.instance.GetTownData().UpdateCurrentGold(goldPerMonth);
    //}

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjSO.GetGridPositionList(orgin, dir);
    }

    public BuildingData GetBuildingData() { return buildingData; }
    public void DestroySelf()
    {
        buildingData.BuildingAI = null;
        Destroy(gameObject);
    }
}

