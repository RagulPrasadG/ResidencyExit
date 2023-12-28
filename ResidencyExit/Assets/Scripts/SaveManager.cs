using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager
{
    private GameDataSO gameDataSO;
    private List<CarStatus> carStatusList = new List<CarStatus>();

    public SaveManager(GameDataSO gameDataSO)
    {
        this.gameDataSO = gameDataSO;
    }

    public void OnApplicationQuit()
    {
        SaveData();
    }

    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
         GetSavedData();

    }

    public void SaveData()
    {          
        SaveCarStatus();
        SaveContainer saveContainer = new SaveContainer
        {
            coinAmount = gameDataSO.coinAmount,
            completedLevels = gameDataSO.completedLevels,
            carstatusList = this.carStatusList 
        };

        string dataString = JsonUtility.ToJson(saveContainer,true);
        string savePath = Application.persistentDataPath + "/saveFile.json";
        Debug.Log(dataString);
        File.WriteAllText(savePath, dataString);
    }

    public void GetSavedData()
    {
        if (gameDataSO.isLoaded) return; //check to avoid loading previously saved data again when returning to mainmenu from gameplay

        if (!File.Exists(Application.persistentDataPath + "/saveFile.json")) return;

       
        string fileString = File.ReadAllText(Application.persistentDataPath + "/saveFile.json");
        SaveContainer saveContainer = JsonUtility.FromJson<SaveContainer>(fileString);
        for (int i = 0; i < saveContainer.carstatusList.Count; i++)
        {
            gameDataSO.carsData[i].carData.carStatus = saveContainer.carstatusList[i];
        }
        gameDataSO.completedLevels = saveContainer.completedLevels;
        gameDataSO.coinAmount = saveContainer.coinAmount;
        gameDataSO.isLoaded = true;
    }

    public void SaveCarStatus()
    {
        carStatusList.Clear();
        for(int i =0;i< gameDataSO.carsData.Count;i++)
        {
            carStatusList.Add(gameDataSO.carsData[i].carData.carStatus);
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

