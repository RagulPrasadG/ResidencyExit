using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<CarDataSO> carData;
    [SerializeField] GameDataSO gameData;
    [SerializeField] LevelDataSO levelDataSO;
    public int collectedCoins;

    public static GameManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);

        Application.targetFrameRate = 60;
    }
   

    public void OnGoalReached()
    {
        gameData.tries = 0;
        StartCoroutine(GamePlayUI.instance.OnGameWin());
    }


    public void IncrementLevel()
    {
        if (gameData.currentLevel < levelDataSO.levelDataCSV.Length - 1)
            gameData.currentLevel++;

    }

    public void OnCoinCollect()
    {
        collectedCoins += 10;
        gameData.coinAmount += collectedCoins;
        
    }

    public void OnCarCrash()
    {
        gameData.tries++;
        if(gameData.tries % 3 == 0)
        {
            AdManager.instance.ShowInterstitialAd();
        }
        AudioManager.instance.PlaySound(4);  //play lose sound here
        StartCoroutine(GamePlayUI.instance.OnGameLose());
    }

}
