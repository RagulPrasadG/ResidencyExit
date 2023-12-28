using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameData",menuName = "Data/NewGameData")]
public class GameDataSO : ScriptableObject
{
    public List<VehicleDataScriptableObject> vehicles;
    public VehicleView playerVehiclePrefab;
    public GameObject[] aiCarPrefab;
    public GameObject[] obstaclePrefab;
    public int currentLevel = 0;
    public int completedLevels = 0;
    public int coinAmount = 100;
    public bool isLoaded = false;
    public int tries = 0;
    public GameObject GetRandomObstaclePrefab()
    {
        return obstaclePrefab[Random.Range(0, obstaclePrefab.Length)];
    }
    public GameObject GetRandomAiCarPrefab()
    {
        return aiCarPrefab[Random.Range(0, aiCarPrefab.Length)];
    }


}
