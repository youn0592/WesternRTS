using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickObject : MonoBehaviour
{
    ObjController objController;
    VilliagerAI villagerAI;

    private void Start()
    {
        objController = GetComponentInParent<ObjController>();
        villagerAI = GetComponentInParent<VilliagerAI>();
    }

    private void OnMouseDown()
    {
        if (Test.Instance.GetPlaceableObject() != null) return;
        if (objController == null && villagerAI == null)
        {
            Debug.LogError("objController or VillagerAI was null");
            return; 
        }

        if (objController) BuildingSelect();
        if (villagerAI) AISelect();

    }

    private void BuildingSelect()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        UIManager.Instance.OnDeselect();
        CameraController.instance.followTransform = transform;
        UIManager.Instance.objectController = objController;
        UIManager.Instance.OnBuildingClicked();
    }

    private void AISelect()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        UIManager.Instance.OnDeselect();
        CameraController.instance.followTransform = transform;
        UIManager.Instance.villagerAI = villagerAI;
        UIManager.Instance.OnAIClicked();
    }
}
