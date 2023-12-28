using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    [SerializeField] GameDataSO gameData;
    [SerializeField] List<CarDataSO> carDataSO;

    public List<CarStatus> carStatusList = new List<CarStatus>();

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
            coinAmount = gameData.coinAmount,
            completedLevels = gameData.completedLevels,
            carstatusList = this.carStatusList 
        };

        string dataString = JsonUtility.ToJson(saveContainer,true);
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
            carDataSO[i].carData.carStatus = saveContainer.carstatusList[i];
        }
        gameData.completedLevels = saveContainer.completedLevels;
        gameData.coinAmount = saveContainer.coinAmount;
        gameData.isLoaded = true;
    }

    public void SaveCarStatus()
    {
        carStatusList.Clear();
        for(int i =0;i< carDataSO.Count;i++)
        {
            carStatusList.Add(carDataSO[i].carData.carStatus);
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

