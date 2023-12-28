using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameService : Singleton<GameService>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameDataSO  gameDataSO;
    [SerializeField] LevelDataSO levelDataSO;
    [SerializeField] SaveServiceScriptableObject  saveServiceSO;
    [SerializeField] AudioServiceScriptableObject audioServiceSO;
    [SerializeField] EventServiceScriptableObject eventServiceSO;
    public int collectedCoins;

    public MainMenuUIService mainMenuUIService;
    public VehicleService vehicleService;

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

    public void SetEvents()
    {
        eventServiceSO.OnCollectCoin.AddListener(OnCoinCollect);
        eventServiceSO.OnReachGoal.AddListener(OnGoalReached);
    }

    public void Init()
    {
        Application.targetFrameRate = 60;
        vehicleService = new VehicleService(gameDataSO,eventServiceSO,audioServiceSO);
        SetEvents();
    }

    public void OnGoalReached()
    {
        gameDataSO.tries = 0;
        StartCoroutine(GamePlayUI.instance.OnGameWin());
    }


    public void IncrementLevel()
    {
        if (gameDataSO.currentLevel < levelDataSO.levelDataCSV.Length - 1)
            gameDataSO.currentLevel++;

    }

    public void OnCoinCollect()
    {
        collectedCoins += 10;
        gameDataSO.coinAmount += collectedCoins;
        
    }

    public void OnCarCrash()
    {
        gameDataSO.tries++;
        if(gameDataSO.tries % 3 == 0)
        {
            AdManager.instance.ShowInterstitialAd();
        }
        audioServiceSO.PlaySFX(audioSource,AudioType.CarCrash);  //play lose sound here
        StartCoroutine(GamePlayUI.instance.OnGameLose());
    }

    public void OnApplicationQuit()
    {
        saveServiceSO.SaveData();
    }
}
