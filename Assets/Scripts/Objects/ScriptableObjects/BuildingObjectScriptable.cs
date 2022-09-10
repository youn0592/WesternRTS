using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectScriptableObject", menuName = "ScriptableObjects/BuildingObject")]
public class BuildingObjectScriptable : ScriptableObject
{
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
                default:
                case Dir.Down: return Dir.Left;
                case Dir.Left: return Dir.Up;
                case Dir.Up: return Dir.Right;
                case Dir.Right: return Dir.Down;    
        }
    }
    public enum Dir
    {
        Down,
        Up,
        Left,
        Right,
    }

    public string buildingName;
    public Transform prefab;
    public Transform visual;
    public int height;
    public int width;



    public int GetRotationAngle(Dir dir)
    {
        switch(dir)
        {
            default:
            case Dir.Down:  return 0;
            case Dir.Left:  return 90;
            case Dir.Up:    return 180;
            case Dir.Right: return 270;
        }
    }
    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch(dir)
        {
            default:
            case Dir.Down:  return new Vector2Int(0, 0);
            case Dir.Left:  return new Vector2Int(0, width);
            case Dir.Up:    return new Vector2Int(width, height);
            case Dir.Right:  return new Vector2Int(height, 0);
        }
    }    
    public List<Vector2Int> GetGridPositionList(Vector2Int offset, Dir dir)
    {
        if (width == 0 || height == 0) Debug.LogError("No Height or Width values for grid in Scriptable Object");

        List<Vector2Int> gridList = new List<Vector2Int> ();

        switch(dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        gridList.Add(offset + new Vector2Int(i, j));
                    }
                }
                break;
            case Dir.Left:
            case Dir.Right:
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        gridList.Add(offset + new Vector2Int(i, j));
                    }
                }
                break;

        }

        return gridList;
    }    
}
