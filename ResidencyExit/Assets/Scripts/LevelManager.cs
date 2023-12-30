using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelDataScriptableObject levelDataSO;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] GameObject cornerWallPrefab;
    [SerializeField] GameDataScriptableObject gameDataSO;
    [SerializeField] GameObject goalRing;

    [Header("Coin")]
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float coinYOffset;

    [Space(10)]
    [SerializeField] CinemachineVirtualCamera followCamera;

    public static LevelManager instance { get; private set; }
    public GameObject carInstance { get; private set; }
   
    public int currentLevel = 0;
    private int aiCarCount = 1;

    public List<AICarController> aicars = new List<AICarController>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

    }


    public void SetLevel()
    {
        for (int i = 0; i < levelDataSO.levelData[gameDataSO.currentLevel].worldgrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelDataSO.levelData[gameDataSO.currentLevel].worldgrid.GetLength(1); j++)
            {
                if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "w" || levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "w\r")
                {
                    GridManager.instance.worldgrid[i, j].isblocked = true;
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));           
                    
                    if(i == 0 && j == 0)
                    {
                        var cornerWall = Instantiate(cornerWallPrefab, Position, Quaternion.identity);
                        cornerWall.transform.eulerAngles = new Vector3(0f, 90, 0f);
                        continue;
                    }
                    else if(i == 0 && j == levelDataSO.levelData[gameDataSO.currentLevel].height - 1)
                    {
                        var cornerWall = Instantiate(cornerWallPrefab, Position, Quaternion.identity);
                        cornerWall.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                        continue;
                    }
                    else if (i == levelDataSO.levelData[gameDataSO.currentLevel].width - 1 &&                             //Setting corner walls of the world grid
                        j == levelDataSO.levelData[gameDataSO.currentLevel].height - 1)
                    {
                        var cornerWall = Instantiate(cornerWallPrefab, Position, Quaternion.identity);
                        cornerWall.transform.eulerAngles = new Vector3(0f, -90f, 0f);
                        continue;
                    }
                    else if (i == levelDataSO.levelData[gameDataSO.currentLevel].width - 1 &&
                        j == 0)
                    {
                        var cornerWall = Instantiate(cornerWallPrefab, Position, Quaternion.identity);
                        cornerWall.transform.eulerAngles = new Vector3(0f, 0f, 0f);
                        continue;
                    }

                    var wall = Instantiate(wallPrefab, Position, Quaternion.identity);
                    if (i == 0 && j < levelDataSO.levelData[gameDataSO.currentLevel].height ||
                        i == levelDataSO.levelData[gameDataSO.currentLevel].width - 1 && j < levelDataSO.levelData[gameDataSO.currentLevel].height)
                    {
                        wall.transform.eulerAngles = new Vector3(0f, 90f, 0f);
                    }
                }

                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "c")
                {
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                    GridManager.instance.HorizontalIndex = i;
                    GridManager.instance.VerticalIndex = j;
                    GameService.Instance.vehicleService.CreateVehicle(Position,followCamera);    
                }

                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "g" || levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "g\r")
                {
                    //Spawn Destination
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                    Instantiate(goalRing, Position, Quaternion.identity);
                    GridManager.instance.worldgrid[i, j].isGoal = true;
                   // levelDataSO.levelData[currentLevel].goalPosition = new Vector2Int(i,j);
                }

                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "o")
                {
                    //Coin Spawn
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                    Position.y += coinYOffset;
                    Instantiate(coinPrefab, Position, Quaternion.identity);
                }
                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "b")
                {
                    GridManager.instance.worldgrid[i, j].isblocked = true;
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                    Instantiate(gameDataSO.GetRandomObstaclePrefab(), Position, Quaternion.identity);
                }

                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == "e")
                {
                    GridManager.instance.worldgrid[i, j].isblocked = true;
                    //exit gate
                }

                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == $"a{aiCarCount}")
                {
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                    var tempcar = Instantiate(gameDataSO.GetRandomAiCarPrefab(), Position, Quaternion.identity);
                    AICarController aicar = tempcar.GetComponent<AICarController>();
                    aicar.carIndex = aiCarCount;
                    aicar.wayPoints.Add(new CarPath(1,Position));
                    aiCarCount++;
                    aicars.Add(aicar);
                }
                else if (levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j] == $"a")
                {
                    Vector3 Position = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                    var tempcar = Instantiate(gameDataSO.GetRandomAiCarPrefab(), Position, Quaternion.identity);
                    tempcar.GetComponent<AICarController>().canMove = false;
                }
            }
        }
        SetAICarPath();

    }
    public void SetAICarPath()
    {
        for (int i = 0; i < levelDataSO.levelData[gameDataSO.currentLevel].worldgrid.GetLength(0); i++)
        {
            for (int j = 0; j < levelDataSO.levelData[gameDataSO.currentLevel].worldgrid.GetLength(1); j++)
            {
                string value = levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j];
                if (value.Length < 4)
                    continue;

                string pathString = levelDataSO.levelData[gameDataSO.currentLevel].worldgrid[i, j];
                int carIndex = 0;
                int carPathNo = 0;
                if (char.IsDigit(pathString[1]))
                {

                    carIndex = GetCharacterAsInt(pathString, 1);
                }
                if (char.IsDigit(pathString[pathString.Length - 1]))
                {
                    carPathNo = GetCharacterAsInt(pathString, pathString.Length - 1);
                }
                
                Vector3 pathPosition = GridManager.instance.GetCellCenter(GridManager.instance.GetCellPosition(i, j));
                aicars[carIndex-1].wayPoints.Add(new CarPath(carPathNo + 1, pathPosition));


            }


        }

    }

    public static int GetCharacterAsInt(string inputString,int index)
    {
        if (inputString.Length >= 2)
        {
            // Get the 2nd character of the string
            char secondCharacter = inputString[index];

            // Convert the 2nd character to an integer using int.Parse()
            if (int.TryParse(secondCharacter.ToString(), out int secondCharacterInt))
            {
                // The 2nd character is a valid number, and its integer value is stored in 'secondCharacterInt'
                return secondCharacterInt;
            }
        }

        return -1; 
    }
}
