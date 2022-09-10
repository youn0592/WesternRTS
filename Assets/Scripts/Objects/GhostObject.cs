using System.Collections;
using System.Collections.Generic;
using System;
using PublicUtility;
using UnityEngine;

public class GhostObject : MonoBehaviour
{

    private BuildingObjectScriptable m_placedObjSO;
    Test m_gridBuildingSystem;
    [SerializeField]
    Material m_GhostMaterial;

    private void Start()
    {
        m_gridBuildingSystem = GameObject.Find("Grid").GetComponent<Test>();

        if (m_gridBuildingSystem == null) Debug.LogError("No Grid Object exists in Scene");
    }

    private void Update()
    {
        if (m_gridBuildingSystem == null) return;

        DrawGhostMesh();
    }

    void DrawGhostMesh()
    {
        m_placedObjSO = m_gridBuildingSystem.GetPlaceableObject();
        Transform pref = m_placedObjSO.prefab;
        MeshFilter mesh = m_placedObjSO.prefab.gameObject.GetComponentInChildren<MeshFilter>();
        if (mesh == null) return;

        Transform newObj = m_placedObjSO.prefab.transform.Find("Cube (12)");
        Vector3 offset = newObj.transform.position;
        Vector3 pos = GetMouseWorldToGrid() + offset;
        Quaternion rot = Quaternion.Euler(0, m_placedObjSO.GetRotationAngle(m_gridBuildingSystem.GetPlaceableObjectDirection()), 0);
        Vector3 scale = newObj.localScale;
        Matrix4x4 matrix = Matrix4x4.TRS(pos, rot, scale);

        Graphics.DrawMesh(mesh.sharedMesh, matrix, m_GhostMaterial, 0);
    }

    public Vector3 GetMouseWorldToGrid()
    {
        Vector3 mousePos = Utils.GetMouseWorldPosition();
        m_gridBuildingSystem.GetGrid().GetCords(mousePos, out int x, out int y);

        if(m_placedObjSO != null)
        {
            Vector2Int rotationOffset = m_placedObjSO.GetRotationOffset(m_gridBuildingSystem.GetPlaceableObjectDirection());
            Vector3 placedObjectWorldPos = m_gridBuildingSystem.GetGrid().GetWorldPos(x, y) + new Vector3(rotationOffset.x, 0, rotationOffset.y);
            return placedObjectWorldPos;
        }
        else
        {
            return mousePos;
        }
    }
}
