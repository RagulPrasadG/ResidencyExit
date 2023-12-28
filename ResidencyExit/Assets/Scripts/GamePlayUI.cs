using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
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

    [SerializeField] GameDataSO gameDataSO;
    [SerializeField] LevelDataSO levelDataSO;

    [SerializeField] Button pauseButton;
    [SerializeField] Button gameWinPanelMenuButton;
    [SerializeField] Button gameWinPanelNextLevelButton;
    [SerializeField] SaveManager saveManager;
    public static GamePlayUI instance { get; private set; }

    private int tweenWinCoinAmount = 0;
    private int tweenLoseCoinAmount = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        AdManager.instance.LoadInterstitialAd();
    }

    public IEnumerator OnGameWin()
    {
        AudioManager.instance.PlaySound(2);

        if (gameDataSO.completedLevels < levelDataSO.levelDataCSV.Length - 1)
        {
            gameDataSO.completedLevels++;
            saveManager.SaveData();
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

            AudioManager.instance.PlaySoundLoop(1,1.4f);
            var tween = DOTween.To(() => tweenWinCoinAmount, TweenWinCoinAmount, GameService.Instance.collectedCoins, 4f);
            tween.onComplete += () =>
            {
                AudioManager.instance.ResetAudioSource();
                
            };

        };


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

            AudioManager.instance.PlaySoundLoop(1, 1.4f);
            var tween = DOTween.To(() => tweenLoseCoinAmount, TweenLoseCoinAmount, GameService.Instance.collectedCoins, 4f);
            tween.onComplete += () =>
            {
                AudioManager.instance.ResetAudioSource();

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
        AudioManager.instance.ResetAudioSource();
        AudioManager.instance.PlayClick();
        GameService.Instance.IncrementLevel();
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnRetryButtonClicked()
    {
        AudioManager.instance.ResetAudioSource();
        AudioManager.instance.PlayClick();
        GameService.Instance.collectedCoins = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnResumeButtonClicked()
    {
        AudioManager.instance.PlayClick();
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
        AudioManager.instance.PlayClick();
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
        AudioManager.instance.PlayClick();
        Time.timeScale = 1;
        GameService.Instance.collectedCoins = 0;
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnMainMenuButtonClicked()
    {
        AudioManager.instance.PlayClick();
        Time.timeScale = 1;
        DOTween.KillAll();
        Destroy(GameService.Instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(AdManager.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void On2XWatchAdButtonClicked()
    {
        if (GameService.Instance.collectedCoins == 0) return;

        AdManager.instance.ShowRewardedAd(GameService.Instance.collectedCoins * 2);
    }

}
