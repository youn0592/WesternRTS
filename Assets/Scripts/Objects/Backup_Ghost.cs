using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backup_Ghost : MonoBehaviour
{
    private Transform visual;
    private BuildingObjectScriptable m_placedObjSO;
    [SerializeField]
    private Material m_ghostMaterial;

    private void Start()
    {
        Test.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
        DrawGhostMesh();
    }


    private void Instance_OnSelectedChanged(object sender, System.EventArgs e)
    {
        DrawGhostMesh();
    }

    private void Update()
    {
        Vector3 targetPos = Test.Instance.GetMouseWorldSnappedPosition();
        targetPos.y = 1f;
        transform.position = targetPos;
        transform.rotation = Test.Instance.GetPlacedObjectRotation();
    }

    private void DrawGhostMesh()
    {
        if (visual != null)
        {
            Destroy(visual.gameObject);
            visual = null;
        }
        BuildingObjectScriptable placeableObjSO = Test.Instance.GetPlaceableObject();
        if (placeableObjSO != null)
        {
            visual = Instantiate(placeableObjSO.visual, Vector3.zero, Quaternion.identity);
            visual.parent = transform;
            visual.localPosition = Vector3.zero;
            visual.localEulerAngles = Vector3.zero;
            visual.GetComponentInChildren<MeshRenderer>().material = m_ghostMaterial;
            SetLayerRecursive(visual.gameObject, 11);
        }
    }

    private void SetLayerRecursive(GameObject targetGameObject, int layer)
    {
        targetGameObject.layer = layer;
        foreach (Transform child in targetGameObject.transform)
        {
            SetLayerRecursive(child.gameObject, layer);
        }
    }
}

