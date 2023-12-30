using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataSO",menuName = "Data/NewData")]
public class LevelDataScriptableObject : ScriptableObject
{
    public LevelData[] levelData;
    public TextAsset[] levelDataCSV;
    public string[,] levelDataString;

    public void ReadDataFromCSV()
    {
        levelData = new LevelData[levelDataCSV.Length];
        for(int i = 0;i<levelDataCSV.Length;i++)
        {
            levelData[i] = new LevelData();
            string[] rows = levelDataCSV[i].ToString().Split('\n');
            int numRows = rows.Length;
            int numColumns = rows[0].Split(',').Length;
            string[,] data = new string[numRows, numColumns];

            for (int j = 0; j < numRows; j++)
            {
                string[] values = rows[j].Split(',');
                for (int k = 0; k < numColumns; k++)
                {
                    data[j, k] = values[k];
                }
            }
            levelData[i].worldgrid = data;
            levelData[i].width = numRows;
            levelData[i].height = numColumns;

            levelData[i].worldgrid = Rotate2DArrayClockwise(levelData[i].worldgrid,5);
         
        }

        
    }
    public static T[,] Rotate2DArrayClockwise<T>(T[,] inputArray, int numRotations)
    {
        int rows = inputArray.GetLength(0);
        int columns = inputArray.GetLength(1);

 
        int effectiveRotations = numRotations % 4;

 
        T[,] rotatedArray = new T[columns, rows];


        for (int rotation = 0; rotation < effectiveRotations; rotation++)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    rotatedArray[j, rows - 1 - i] = inputArray[i, j];
                }
            }

           
            inputArray = rotatedArray;

            int temp = rows;
            rows = columns;
            columns = temp;
        }

        return rotatedArray;
    }
}
[System.Serializable]
public struct LevelData
{
    public int width;
    public int height;
    public string[,] worldgrid;
}