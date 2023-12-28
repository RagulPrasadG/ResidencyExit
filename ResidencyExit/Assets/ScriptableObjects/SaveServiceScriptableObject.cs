using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[CreateAssetMenu(fileName = "NewService",menuName = "Data/NewService")]
public class SaveServiceScriptableObject : ScriptableObject
{
    public GameDataSO gameData;
    private List<CarStatus> carStatusList = new List<CarStatus>();

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
        Debug.Log(dataString);
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
            gameData.carsData[i].carData.carStatus = saveContainer.carstatusList[i];
        }
        gameData.completedLevels = saveContainer.completedLevels;
        gameData.coinAmount = saveContainer.coinAmount;
        gameData.isLoaded = true;
    }

    public void SaveCarStatus()
    {
        carStatusList.Clear();
        for (int i = 0; i < gameData.carsData.Count; i++)
        {
            carStatusList.Add(gameData.carsData[i].carData.carStatus);
        }
    }
}

[System.Serializable]
public class SaveContainer
{
    public List<CarStatus> carstatusList = new List<CarStatus>();
    public int coinAmount;
    public int completedLevels;
}