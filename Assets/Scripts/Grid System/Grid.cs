using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



public enum EGridLayout
{
    XY,
    YZ,
    XZ,
}
public class Grid<TGridObject>
{
    public event EventHandler<OnGridChangedEventArgs> OnGridEvent;
    public class OnGridChangedEventArgs : EventArgs { public int x, y; }

    private EGridLayout m_gridLayout;
    private int m_Width;
    private int m_Height;
    private float m_CellSize;
    private Vector3 m_OriginPos;
    private TGridObject[,] m_gridArray;
    
    //Constructor
    public Grid(int width, int height, float cellSize, Vector3 originPos, Func<Grid<TGridObject>, int, int, TGridObject> createGridObj,  EGridLayout layout)
    {
        m_Width = width;
        m_Height = height;
        m_CellSize = cellSize;
        m_OriginPos = originPos;
        m_gridLayout = layout;

        m_gridArray = new TGridObject[width, height];

        for (int i = 0; i < m_Width; i++)
        {
            for (int j = 0; j < m_Height; j++)
            {
                m_gridArray[i, j] = createGridObj(this, i, j);
            }
        }
                ContructGrid();
    }

    //Function to Construct the grid using a nested for loop.
    public void ContructGrid()
    {
        for (int i = 0; i < m_Width; i++)
        {
            for (int j = 0; j < m_Height; j++)
            {
                Debug.DrawLine(GetWorldPos(i, j), GetWorldPos(i, j + 1), Color.white, 100.0f);
                Debug.DrawLine(GetWorldPos(i, j), GetWorldPos(i + 1, j), Color.white, 100.0f);
            }
        }

        Debug.DrawLine(GetWorldPos(0, m_Height), GetWorldPos(m_Width, m_Height), Color.white, 100.0f);
        Debug.DrawLine(GetWorldPos(m_Width, 0), GetWorldPos(m_Width, m_Height), Color.white, 100.0f);

    }

    //Function for getting the world position of a cell.
    public Vector3 GetWorldPos(int x, int y)
    {
        switch(m_gridLayout)
        {
            case EGridLayout.XY:
                return new Vector3(x, y) * m_CellSize + m_OriginPos;
            case EGridLayout.YZ:
                return new Vector3(0, x, y) * m_CellSize + m_OriginPos;
            case EGridLayout.XZ:
                return new Vector3(x, 0, y) * m_CellSize + m_OriginPos;
        }

        Debug.LogError("GridLayout was null in GetWorldPos()");
        return Vector3.zero;
    }

    //Function for getting the cordniates of a cell from a world position.
    public void GetCords(Vector3 worldPosition, out int x, out int y)
    {
        switch(m_gridLayout)
        {
            case EGridLayout.XY:
                x = Mathf.FloorToInt((worldPosition - m_OriginPos).x / m_CellSize);
                y = Mathf.FloorToInt((worldPosition - m_OriginPos).y / m_CellSize);
                break;
            case EGridLayout.YZ:
                x = Mathf.FloorToInt((worldPosition - m_OriginPos).y / m_CellSize);
                y = Mathf.FloorToInt((worldPosition - m_OriginPos).z / m_CellSize);
                break;
            case EGridLayout.XZ:
                x = Mathf.FloorToInt((worldPosition - m_OriginPos).x / m_CellSize);
                y = Mathf.FloorToInt((worldPosition - m_OriginPos).z / m_CellSize);
                break;
            default:
                x = 0;
                y = 0;
                Debug.LogError("Grid Layout was null in GetCords");
                break;
        }
    }

    //Function to set a value in a cell using X and Y cords.
    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x >= 0 && y >= 0 && x < m_Width && y < m_Height)
        {
            m_gridArray[x, y] = value;
            if (OnGridEvent != null) OnGridEvent(this, new OnGridChangedEventArgs {x = x, y = y});
            //Debug.Log("X: " + x + " Y: " + y + " Value: " + m_gridArray[x,y]);
        }
    }

    public void TriggerGridObjectEvent(int x, int y)
    {
        if (OnGridEvent != null) OnGridEvent(this, new OnGridChangedEventArgs { x = x, y = y });
    }

    //Function to set a value in a cell using World Position.
    public void SetGridObject(Vector3 worldPos, TGridObject value)
    {
        int x, y;
        GetCords(worldPos, out x, out y);

        SetGridObject(x, y, value);
    }

    //Function to return a value of a cell using X and Y.
    public TGridObject GetGridObject (int x, int y)
    {
        if(x >= 0 && y >= 0 && x < m_Width && y < m_Height)
        {
            return m_gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    //Function to return a value of a using World Position.
    public TGridObject GetGridObject(Vector3 worldPos)
    {
        int x, y;
        GetCords(worldPos, out x, out y);
        return GetGridObject(x, y);
    }

    public float GetCellSize()
    {
        return m_CellSize;
    }
    
}
