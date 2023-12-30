using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUIService : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] RectTransform gameWinPanel;
    [SerializeField] RectTransform gameplayHUD;
    [SerializeField] RectTransform gameLosePanel;
    [SerializeField] RectTransform pauseGamePanel;

    [Space(10)]
    [SerializeField] Transform centerPoint;
    [SerializeField] Transform bottomPoint;

    [SerializeField] float animationDuration;
    [SerializeField] TMPro.TMP_Text coinCountText;
    [SerializeField] TMPro.TMP_Text loseCoinCountText;

    [Space(10)]
    [Header("ScriptableObjects")]
    [SerializeField] GameDataScriptableObject gameDataSO;
    [SerializeField] LevelDataScriptableObject levelDataSO;
    [SerializeField] AudioServiceScriptableObject audioServiceSO;
    [SerializeField] SaveServiceScriptableObject saveServiceSO;
    [SerializeField] EventServiceScriptableObject eventServiceSO;

    [Space(10)]
    [Header("HUD Buttons")]
    [SerializeField] Button pauseButton;
    [SerializeField] Button gameWinPanelMenuButton;
    [SerializeField] Button gameWinPanelNextLevelButton;

    [Space(10)]
    [Header("Direction Buttons")]
    [SerializeField] Button upButton;
    [SerializeField] Button downButton;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;

    [Space(10)]
    [SerializeField] AudioSource audioSource;


    private int tweenWinCoinAmount = 0;
    private int tweenLoseCoinAmount = 0;

    private void Awake()
    {
        AdManager.instance.LoadInterstitialAd();
        SetEvents();
    }

    public IEnumerator OnGameWin()
    {
        audioServiceSO.PlaySFX(audioSource,AudioType.LevelUp);
        if (gameDataSO.completedLevels < levelDataSO.levelDataCSV.Length - 1)
        {
            gameDataSO.completedLevels++;
            saveServiceSO.SaveData();
        }


        if (gameDataSO.currentLevel >= levelDataSO.levelDataCSV.Length - 1)
        {
            gameWinPanelNextLevelButton.gameObject.SetActive(false);  //Only show the mainmenu button and retry button if we reached the final level
            gameWinPanelMenuButton.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(1.5f);
    

        if (gameDataSO.currentLevel % 3 == 0 && Application.internetReachability != NetworkReachability.NotReachable)
        {
            AdManager.instance.ShowInterstitialAd();
        }

        pauseButton.gameObject.SetActive(false);
        gameplayHUD.gameObject.SetActive(false);
        gameWinPanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(gameWinPanel.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            if (GameService.Instance.collectedCoins == 0) return;

            audioServiceSO.PlaySoundLoop(audioSource,AudioType.CoinCollect, 1.4f);
            var tween = DOTween.To(() => tweenWinCoinAmount, TweenWinCoinAmount, GameService.Instance.collectedCoins, 4f);
            tween.onComplete += () =>
            {
                audioServiceSO.ResetAudioSource(audioSource);
                
            };

        };


    }

    public void SetEvents()
    {
        upButton.onClick.AddListener(OnUpButtonClicked);
        downButton.onClick.AddListener(OnDownButtonClicked);
        leftButton.onClick.AddListener(OnLeftButtonClicked);
        rightButton.onClick.AddListener(OnRightButtonClicked);
    }

    public IEnumerator OnGameLose()
    {
        pauseButton.gameObject.SetActive(false);
        DOTween.KillAll();
        yield return new WaitForSeconds(1.5f);
        
        gameplayHUD.gameObject.SetActive(false);
        gameLosePanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(gameLosePanel.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            if (GameService.Instance.collectedCoins == 0) return;

            audioServiceSO.PlaySoundLoop(audioSource,AudioType.CoinCollect, 1.4f);
            var tween = DOTween.To(() => tweenLoseCoinAmount, TweenLoseCoinAmount, GameService.Instance.collectedCoins, 4f);
            tween.onComplete += () =>
            {
                audioServiceSO.ResetAudioSource(audioSource);

            };

        };

    }

    private void TweenWinCoinAmount(int x)
    {
        tweenWinCoinAmount = x;
        coinCountText.text = $"+ {x}";
    }

    private void TweenLoseCoinAmount(int x)
    {
        tweenLoseCoinAmount = x;
        loseCoinCountText.text = $"+ {x}";
    }

    public void OnNextLevelButtonClicked()
    {
        audioServiceSO.ResetAudioSource(audioSource);
        audioServiceSO.PlaySFX(audioSource,AudioType.ButtonClick);
        GameService.Instance.IncrementLevel();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnRetryButtonClicked()
    {
        audioServiceSO.ResetAudioSource(audioSource);
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        GameService.Instance.collectedCoins = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnResumeButtonClicked()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        Time.timeScale = 1;
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(pauseGamePanel.transform.DOMove(bottomPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            pauseGamePanel.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
        };
        
    }

    public void OnPauseButtonClicked()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        pauseButton.gameObject.SetActive(false);
        pauseGamePanel.gameObject.SetActive(true);    
        var tween = pauseGamePanel.transform.DOMove(centerPoint.position, animationDuration);
        tween.onComplete += ()
           =>
         {
             Time.timeScale = 0;
         };
    }
    public void OnRestartButtonClicked()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        Time.timeScale = 1;
        GameService.Instance.collectedCoins = 0;
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonClicked()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        Time.timeScale = 1;
        DOTween.KillAll();
        Destroy(GameService.Instance.gameObject);
        Destroy(AdManager.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnLeftButtonClicked() => eventServiceSO.OnMoveVehicle.RaiseEvent(VehicleMoveDirection.Left);
    public void OnRightButtonClicked() => eventServiceSO.OnMoveVehicle.RaiseEvent(VehicleMoveDirection.Right);
    public void OnUpButtonClicked() => eventServiceSO.OnMoveVehicle.RaiseEvent(VehicleMoveDirection.Up);
    public void OnDownButtonClicked() => eventServiceSO.OnMoveVehicle.RaiseEvent(VehicleMoveDirection.Down);


    public void On2XWatchAdButtonClicked()
    {
        if (GameService.Instance.collectedCoins == 0) return;

        AdManager.instance.ShowRewardedAd(GameService.Instance.collectedCoins * 2);
    }

}
