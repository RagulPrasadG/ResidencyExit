using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameService : Singleton<GameService>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameDataSO gameData;
    [SerializeField] LevelDataSO levelDataSO;
    [SerializeField] SaveServiceScriptableObject saveServiceSO;
    [SerializeField] AudioServiceScriptableObject soundServiceSO;
    public int collectedCoins;

    public UiManager uIManager;
    public SaveManager saveManager;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        Init();
    }

    public void Start()
    {
        saveServiceSO.LoadData();
    }

    public void Init()
    {
        Application.targetFrameRate = 60;
        uIManager.Init(saveManager);
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
        soundServiceSO.PlaySFX(audioSource,AudioType.CarCrash);  //play lose sound here
        StartCoroutine(GamePlayUI.instance.OnGameLose());
    }

    public void OnApplicationQuit()
    {
        saveServiceSO.SaveData();
    }
}
