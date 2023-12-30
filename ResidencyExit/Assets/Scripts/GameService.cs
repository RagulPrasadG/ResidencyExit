using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameService : Singleton<GameService>
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameDataScriptableObject  gameDataSO;
    [SerializeField] LevelDataScriptableObject levelDataSO;
    [SerializeField] SaveServiceScriptableObject  saveServiceSO;
    [SerializeField] AudioServiceScriptableObject audioServiceSO;
    [SerializeField] EventServiceScriptableObject eventServiceSO;
    public int collectedCoins;

    public MainMenuUIService mainMenuUIService { get; set; }
    public GamePlayUIService gameplayUIService { get; set; }
    public VehicleService vehicleService { get; set; }

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
        eventServiceSO.OnCollectCoin.RemoveAllListeners();
        eventServiceSO.OnReachGoal.RemoveAllListeners();
        eventServiceSO.OnCollectCoin.AddListener(OnCoinCollect);
        eventServiceSO.OnReachGoal.AddListener(OnGoalReached);
    }

    public void Init()
    {
        SceneManager.sceneLoaded += OnSceneChange;
        Application.targetFrameRate = 60;
        vehicleService = new VehicleService(gameDataSO,eventServiceSO,audioServiceSO);
        SetEvents();
    }

    public void OnSceneChange(Scene scene,LoadSceneMode sceneMode)
    {
        if (scene.name == "MainMenu")
        {
            gameplayUIService = null;
            mainMenuUIService = FindObjectOfType<MainMenuUIService>();
        }
        else if (scene.name == "Gameplay")
        {
            mainMenuUIService = null;
            gameplayUIService = FindObjectOfType<GamePlayUIService>();
        }
    }

    public void OnGoalReached()
    {
        gameDataSO.tries = 0;
        StartCoroutine(gameplayUIService.OnGameWin());
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
        StartCoroutine(gameplayUIService.OnGameLose());
    }

    public void OnApplicationQuit()
    {
        saveServiceSO.SaveData();
    }
}
