using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Transform levelList;
    [Header("Panels")]
    [SerializeField] Transform mainMenuPanel;
    [SerializeField] Transform shopPanel;
    [SerializeField] Transform watchAdPanel;

    [SerializeField] Transform centerPoint;
    [SerializeField] Transform bottomPoint;
    [SerializeField] Transform topPoint;

    [SerializeField] TMPro.TMP_Text coinText;

    [SerializeField] GameDataSO gameDataSO;
    [SerializeField] SaveServiceScriptableObject saveServiceSO;
    [SerializeField] AudioServiceScriptableObject audioServiceSO;

    [SerializeField] Button coinsCheatButton;
    [SerializeField] AudioSource audioSource;
    
    private SaveManager saveManager;

    public float animationDuration  = 0.3f;



    private void Start()
    {
#if DEV
        coinsCheatButton.gameObject.SetActive(true);
#else
        coinsCheatButton.gameObject.SetActive(false);
#endif
        SetCoinText();

    }

    public void Init(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

    public void OpenLevelList()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        levelList.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(mainMenuPanel.transform.DOMove(bottomPoint.position, animationDuration));
        animationSequence.Append(levelList.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            mainMenuPanel.gameObject.SetActive(false);
        };
    }

    public void OnBackButtonClickFromLevelList()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        mainMenuPanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(levelList.transform.DOMove(topPoint.position, animationDuration));
        animationSequence.Append(mainMenuPanel.transform.DOMove(centerPoint.position, animationDuration)); 
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            levelList.gameObject.SetActive(false);
        };
    }
    public void OnBackButtonClickFromShopPanel()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        mainMenuPanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(shopPanel.transform.DOMove(topPoint.position, animationDuration));
        animationSequence.Append(mainMenuPanel.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            shopPanel.gameObject.SetActive(false);
        };
    }

    public void OnBackButtonClickFromWatchADPanel()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        shopPanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(watchAdPanel.transform.DOMove(topPoint.position, animationDuration));
        animationSequence.Append(shopPanel.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            watchAdPanel.gameObject.SetActive(false);
        };
    }

    public void OpenWatchAdPanel()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        watchAdPanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(shopPanel.transform.DOMove(bottomPoint.position, animationDuration));
        animationSequence.Append(watchAdPanel.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            shopPanel.gameObject.SetActive(false);
        };
    }


    public void OpenShop()
    {
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        shopPanel.gameObject.SetActive(true);
        Sequence animationSequence = DOTween.Sequence();
        animationSequence.Append(mainMenuPanel.transform.DOMove(bottomPoint.position, animationDuration));
        animationSequence.Append(shopPanel.transform.DOMove(centerPoint.position, animationDuration));
        animationSequence.Play();
        animationSequence.onComplete += () =>
        {
            mainMenuPanel.gameObject.SetActive(false);
        };
    }

    public void SetCoinText()
    {
        coinText.text = gameDataSO.coinAmount.ToString();
        saveServiceSO.SaveData();
    }

    public void TweenCoinVisual(int toValue)
    {
        if (toValue > gameDataSO.coinAmount) return;


        int tweenCoinAmount = gameDataSO.coinAmount;
        int endValue = gameDataSO.coinAmount - toValue;

        var tween = DOTween.To(() => tweenCoinAmount, SetCoinVisual
        , endValue, 4f);

        
        
    }

    private void SetCoinVisual(int x)
    {
        coinText.text = $"{x}";
    }


//#if Live
//    public void IncreaseCoinCheat()
//    {
//        gameDataSO.coinAmount += 1000;
//        coinText.text = gameDataSO.coinAmount.ToString();
//    }
//#endif

    public void ExitGame()
    {
        //Save
        audioServiceSO.PlaySFX(audioSource, AudioType.ButtonClick);
        Application.Quit();
    }


}
