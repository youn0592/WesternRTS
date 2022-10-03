using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using PublicUtility;

public class Test : MonoBehaviour
{
    public static Test Instance { get; private set; }

    GameManager gameManager;

    [SerializeField]
    private List<BuildingObjectScriptable> placeableObjectList;
    private BuildingObjectScriptable placeableObject;
    private BuildingObjectScriptable.Dir dir = BuildingObjectScriptable.Dir.Down;
    private bool bCanDelete = true;

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    [SerializeField]
    private Material ghostMaterial;

    Grid<GridObject> m_Grid;
    public EGridLayout gridLayout;
    public int Width = 20;
    public int Length = 20;
    public float CellSize = 5.0f;


    void Awake()
    {
        Instance = this;

        m_Grid = new Grid<GridObject>(Width, Length, CellSize, transform.position, (Grid<GridObject> gd, int x, int y) => new GridObject(gd, x, y), gridLayout);
        placeableObject = null;

        gameManager = GameObject.Find("_GAMEMANAGER_").GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager was not found");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (placeableObject == null) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;

            m_Grid.GetCords(Utils.GetMouseWorldPosition(), out int x, out int y);
            List<Vector2Int> GridList = placeableObject.GetGridPositionList(new Vector2Int(x, y), dir);

            GridObject gridObj = m_Grid.GetGridObject(x, y);

            //Test if Building can be placed near object
            bool canBuild = true;
            foreach (Vector2Int gridPos in GridList)
            {
                if (m_Grid.GetGridObject(gridPos.x, gridPos.y) == null || !m_Grid.GetGridObject(gridPos.x, gridPos.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }
            if (placeableObject.prefab.GetComponent<BuildingData>().GetGoldCost() > gameManager.GetTownData().GetCurrentGold())
            {
                canBuild = false;
                GameManager.instance.GetUIManager().NotEnoughGold();
                DeselectObjectType();
                return;
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = placeableObject.GetRotationOffset(dir);
                Vector3 PlacedObjectWorldPosition = m_Grid.GetWorldPos(x, y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * m_Grid.GetCellSize();

                ObjController placedObject = ObjController.Create(PlacedObjectWorldPosition, new Vector2Int(x, y), dir, placeableObject);

                foreach (Vector2Int p in GridList)
                {
                    m_Grid.GetGridObject(p.x, p.y).SetObject(placedObject);
                }

                gameManager.GetTownData().UpdateCurrentGold(-placedObject.GetGoldCost());
                OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                gameManager.AddBuildingToList(placedObject.GetBuildingData());
                bCanDelete = false;
                DeselectObjectType();
            }
            else
            {
                Debug.Log("Can't place building here");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!bCanDelete) return;
            GridObject gridObject = m_Grid.GetGridObject(Utils.GetMouseWorldPosition());
            if (gridObject == null) return;
            ObjController placedObj = gridObject.GetObject();
            if (placedObj != null)
            {
                gameManager.RemoveBuildingFromList(placedObj.GetBuildingData());
                placedObj.DestroySelf();

                List<Vector2Int> GridList = placedObj.GetGridPositionList();

                foreach (Vector2Int p in GridList)
                {
                    m_Grid.GetGridObject(p.x, p.y).ClearObject();
                }
            }

        }

        //temp code for a Delete feature
        //if (Input.GetKey(KeyCode.P))
        //{
        //    bCanDelete = !bCanDelete;
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = BuildingObjectScriptable.GetNextDir(dir);

        }

        //if (Input.GetKeyDown(KeyCode.Alpha1)) { placeableObject = placeableObjectList[0]; RefreshSelectedObjectType(); bCanDelete = true; }
        //if (Input.GetKeyDown(KeyCode.Alpha2)) { placeableObject = placeableObjectList[1]; RefreshSelectedObjectType(); bCanDelete = true; }
        //if (Input.GetKeyDown(KeyCode.Alpha3)) { placeableObject = placeableObjectList[2]; RefreshSelectedObjectType(); bCanDelete = true; }
        //if (Input.GetKeyDown(KeyCode.Alpha4)) { placeableObject = placeableObjectList[3]; RefreshSelectedObjectType(); bCanDelete = true; }

        if (Input.GetKeyDown(KeyCode.Escape)) { DeselectObjectType(); bCanDelete = false; }
    }

    public void SetPlaceableObj(int index)
    {
        placeableObject = placeableObjectList[index];
        RefreshSelectedObjectType();
        bCanDelete = true;
    }

    public BuildingObjectScriptable GetPlaceableObject()
    {
        return placeableObject;
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placeableObject != null)
        {
            return Quaternion.Euler(0, placeableObject.GetRotationAngle(dir), 0);
        }
        else
            return Quaternion.identity;
    }
    public BuildingObjectScriptable.Dir GetPlaceableObjectDirection()
    {
        return dir;
    }

    public Grid<GridObject> GetGrid()
    {
        return m_Grid;
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Utils.GetMouseWorldPosition();
        m_Grid.GetCords(mousePosition, out int x, out int z);

        if (placeableObject != null)
        {
            Vector2Int rotationOffset = placeableObject.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = m_Grid.GetWorldPos(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * m_Grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    private void DeselectObjectType()
    {
        placeableObject = null;
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }

}



public class GridObject
{
    private Grid<GridObject> m_grid;
    private int x;
    private int y;
    private ObjController m_PlacedObject;

    public GridObject(Grid<GridObject> grid, int x, int y)
    {
        m_grid = grid;
        this.x = x;
        this.y = y;
    }

    public void SetObject(ObjController placeObject)
    {
        m_PlacedObject = placeObject;
        m_grid.TriggerGridObjectEvent(x, y);
    }

    public ObjController GetObject()
    {
        return m_PlacedObject;
    }

    public void ClearObject()
    {
        m_PlacedObject = null;
        m_grid?.TriggerGridObjectEvent(x, y);
    }

    public bool CanBuild()
    {
        return m_PlacedObject == null;
    }

}


