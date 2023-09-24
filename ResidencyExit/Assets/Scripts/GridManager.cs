using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridManager : MonoBehaviour
{
    public Cell[,] worldgrid;
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float cellsize;
    public int VerticalIndex;
    public int HorizontalIndex;


    [SerializeField] LevelDataSO levelDataSO;
    [SerializeField] GameDataSO gameDataSO;
    public static GridManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        CreateGrid();
    }


    private void CreateGrid()
    {
        levelDataSO.ReadDataFromCSV();
        width = levelDataSO.levelData[gameDataSO.currentLevel].width;
        height = levelDataSO.levelData[gameDataSO.currentLevel].height;

        worldgrid = new Cell[width, height];
       
        for (int i = 0;i < worldgrid.GetLength(0); i++)
        {
            for(int j = 0;j < worldgrid.GetLength(1); j++)
            {
               
                worldgrid[i, j] = new Cell(GetCellPosition(i, j), false);
#if UNITY_EDITOR
                Debug.DrawLine(worldgrid[i, j].position, GetCellPosition(i, j + 1), Color.green, 300f);
                Debug.DrawLine(worldgrid[i, j].position, GetCellPosition(i + 1, j), Color.green, 300f);
#endif
            }
        }
#if UNITY_EDITOR
        Debug.DrawLine(GetCellPosition(0, height), GetCellPosition(width, height), Color.green, 300f);
        Debug.DrawLine(GetCellPosition(width, 0), GetCellPosition(width, height), Color.green, 300f);
#endif
        LevelManager.instance.SetLevel();
    }

  
    public Vector3 GetCellPosition(int x, int y)
    {
        return new Vector3(x * cellsize, 0f, y * cellsize);
    }
    public Vector3 GetCellCenter(Vector3 cellposition)
    {
        return cellposition + new Vector3(cellsize, 0f, cellsize) * 0.5f;
    }

    public Vector3 GetUpEndPosition()
    {
        int index = 0;
        for(int y = VerticalIndex;y < height;y++)
        {
            //var goalIndex = levelDataSO.levelData[gameDataSO.currentLevel].goalPosition;
            //if (y == goalIndex.y && HorizontalIndex == goalIndex.x)
            //{
            //    return GetCellCenter(GetCellPosition(goalIndex.x, goalIndex.y));
            //}
            if (worldgrid[HorizontalIndex,y].isGoal)
            {
                return GetCellCenter(GetCellPosition(HorizontalIndex, y));
            }

            if (worldgrid[HorizontalIndex,y].isblocked == true)
            {
                if (y - 1 == VerticalIndex)
                    return Vector3.zero;        //return zero if the position is same as the car's position

                VerticalIndex = y - 1;
                return GetCellCenter(GetCellPosition(HorizontalIndex,VerticalIndex));
            }
            index = y;
        }
        VerticalIndex = index;
        return GetCellCenter(GetCellPosition(HorizontalIndex, index));
    }

    public Vector3 GetDownEndPosition()
    {
        int index = 0;
        for (int y = VerticalIndex; y >= 0; y--)
        {
            //var goalIndex = levelDataSO.levelData[gameDataSO.currentLevel].goalPosition;
            //if (y == goalIndex.y && HorizontalIndex == goalIndex.x)
            //{
            //    return GetCellCenter(GetCellPosition(goalIndex.x, goalIndex.y));
            //}
            if (worldgrid[HorizontalIndex, y].isGoal)
            {
                return GetCellCenter(GetCellPosition(HorizontalIndex, y));
            }

            if (worldgrid[HorizontalIndex, y].isblocked)
            {
                if (y + 1 == VerticalIndex)
                    return Vector3.zero;

                VerticalIndex = y + 1;
                return GetCellCenter(GetCellPosition(HorizontalIndex, VerticalIndex));
            }
            index = y;
        }
        VerticalIndex = index;
        return GetCellCenter(GetCellPosition(HorizontalIndex, index));
    }

    public Vector3 GetRightEndPosition()
    {
        int index = 0;
        for (int x = HorizontalIndex; x < width; x++)
        {
            //var goalIndex = levelDataSO.levelData[gameDataSO.currentLevel].goalPosition;
            //if (x == goalIndex.x && VerticalIndex == goalIndex.y)
            //{
            //    return GetCellCenter(GetCellPosition(goalIndex.x, goalIndex.y));
            //}

            if (worldgrid[x, VerticalIndex].isGoal)
            {
                return GetCellCenter(GetCellPosition(x, VerticalIndex));
            }

            if (worldgrid[x, VerticalIndex].isblocked == true)
            {
                if (x - 1 == HorizontalIndex)
                    return Vector3.zero;

                HorizontalIndex = x - 1;
                return GetCellCenter(GetCellPosition(HorizontalIndex, VerticalIndex));
            }
            index = x;
        }
        HorizontalIndex = index;
        return GetCellCenter(GetCellPosition(index, VerticalIndex));
    }

    public Vector3 GetLeftEndPosition()
    {
        int index = 0;
        for (int x = HorizontalIndex; x >= 0 ; x--)
        {
            //var goalIndex = levelDataSO.levelData[gameDataSO.currentLevel].goalPosition;
            //if (x == goalIndex.x && VerticalIndex == goalIndex.y)
            //{
            //    return GetCellCenter(GetCellPosition(goalIndex.x, goalIndex.y));
            //}

            if (worldgrid[x, VerticalIndex].isGoal)
            {
                return GetCellCenter(GetCellPosition(x, VerticalIndex));
            }

            if (worldgrid[x, VerticalIndex].isblocked == true)
            {
                if (x + 1 == HorizontalIndex)
                    return Vector3.zero;

                HorizontalIndex = x + 1;
                return GetCellCenter(GetCellPosition(HorizontalIndex, VerticalIndex));
            }
            index = x;
        }
        HorizontalIndex = index;
        return GetCellCenter(GetCellPosition(index, VerticalIndex));
    }

}

public class Cell
{
    public Cell(Vector3 position,bool isblocked)
    {
        this.position = position;
        this.isblocked = isblocked;

    }
    public Vector3 position;
    public bool isblocked;
    public bool isGoal;
}
