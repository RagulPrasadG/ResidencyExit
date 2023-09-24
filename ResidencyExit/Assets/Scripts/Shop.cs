using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    
    [SerializeField]RectTransform shopContentParent;
    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] GameDataSO gameDataSO;
    [SerializeField] UiManager uiManager;
    [SerializeField] SaveManager saveManager;
    public List<ShopItem> shopItems = new List<ShopItem>();

    private void Start()
    {
        InitShop();
    }

    private void InitShop()
    {
        foreach(CarDataSO car in GameManager.instance.carData)
        {
            var temp = Instantiate(shopItemPrefab);
            temp.transform.SetParent(shopContentParent.transform, false);
            ShopItem item = temp.GetComponent<ShopItem>();

            if(car.carData.carStatus == CarStatus.Equipped)
            {
                gameDataSO.playercarPrefab = car.carData.carPrefab;
            }

            shopItems.Add(item);
            item.InitItem(car);
            item.button.onClick.AddListener(delegate { OnSelectItem(item); });
            
        }
    }

    public void OnSelectItem(ShopItem item)
    {
        AudioManager.instance.PlayClick();
            switch (item.carDataSO.carData.carStatus)
            {
                case CarStatus.Buy:
                //implement buy based on funds
                if(item.carDataSO.carData.amount <= gameDataSO.coinAmount)
                {
                    uiManager.TweenCoinVisual(item.carDataSO.carData.amount);
                    gameDataSO.coinAmount -= item.carDataSO.carData.amount;
                    item.ToggleButtonText(CarStatus.Equip);
                    item.carDataSO.carData.carStatus = CarStatus.Equip;
                    AudioManager.instance.PlaySound(3);
                    saveManager.SaveData();
                    //uiManager.SetCoinText();
                }
                else
                {
                    //toggle not enough coins panel to watch ad
                    uiManager.OpenWatchAdPanel();
                }
                    break;
                case CarStatus.Equip:
                 foreach(ShopItem shopitem in shopItems)
                 {
                    if(shopitem.carDataSO.carData.carStatus == CarStatus.Equipped)
                    {
                        shopitem.carDataSO.carData.carStatus = CarStatus.Equip;
                        shopitem.ToggleButtonText(CarStatus.Equip);
                    }
                 }
                item.carDataSO.carData.carStatus = CarStatus.Equipped;
                item.ToggleButtonText(CarStatus.Equipped);
                gameDataSO.playercarPrefab = item.carDataSO.carData.carPrefab;
                saveManager.SaveData();
                break;
        }
      
    }
   

}
