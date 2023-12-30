using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[CreateAssetMenu(fileName = "NewService",menuName = "Data/NewService")]
public class SaveServiceScriptableObject : ScriptableObject
{
    public GameDataScriptableObject gameData;
    private List<VehicleStatus> carStatusList = new List<VehicleStatus>();

    public void LoadData()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            GetSavedData();
    }

    public void SaveData()
    {
        SaveCarStatus();
        SaveContainer saveContainer = new SaveContainer
        {
            coinAmount = gameData.coinAmount,
            completedLevels = gameData.completedLevels,
            carstatusList = this.carStatusList
        };

        string dataString = JsonUtility.ToJson(saveContainer, true);
        string savePath = Application.persistentDataPath + "/saveFile.json";
        File.WriteAllText(savePath, dataString);
    }

    public void GetSavedData()
    {
        if (gameData.isLoaded) return; //check to avoid loading previously saved data again when returning to mainmenu from gameplay

        if (!File.Exists(Application.persistentDataPath + "/saveFile.json")) return;


        string fileString = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
        SaveContainer saveContainer = JsonUtility.FromJson<SaveContainer>(fileString);
        for (int i = 0; i < saveContainer.carstatusList.Count; i++)
        {
            gameData.vehicles[i].vehicleData.vehicleStatus = saveContainer.carstatusList[i];
        }
        gameData.completedLevels = saveContainer.completedLevels;
        gameData.coinAmount = saveContainer.coinAmount;
        gameData.isLoaded = true;
    }

    public void SaveCarStatus()
    {
        carStatusList.Clear();
        for (int i = 0; i < gameData.vehicles.Count; i++)
        {
            carStatusList.Add(gameData.vehicles[i].vehicleData.vehicleStatus);
        }
    }
}

[System.Serializable]
public class SaveContainer
{
    public List<VehicleStatus> carstatusList = new List<VehicleStatus>();
    public int coinAmount;
    public int completedLevels;
}