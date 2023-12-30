using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    
    [SerializeField] RectTransform shopContentParent;
    [SerializeField] GameObject shopItemPrefab;
    [SerializeField] SaveServiceScriptableObject saveServiceSO;
    [SerializeField] AudioServiceScriptableObject audioServiceSO;
    [SerializeField] GameDataSO gameDataSO;
    [SerializeField] AudioSource audioSource;
    public List<ShopItem> shopItems = new List<ShopItem>();

    private void Start()
    {
        InitShop();
    }

    private void InitShop()
    {
        foreach(VehicleDataScriptableObject car in gameDataSO.vehicles)
        {
            var temp = Instantiate(shopItemPrefab);
            temp.transform.SetParent(shopContentParent.transform, false);
            ShopItem item = temp.GetComponent<ShopItem>();

            if(car.vehicleData.vehicleStatus == VehicleStatus.Equipped)
            {
                gameDataSO.playerVehiclePrefab = car.vehicleData.vehiclePrefab;
            }

            shopItems.Add(item);
            item.InitItem(car);
            item.button.onClick.AddListener(delegate { OnSelectItem(item); });
            
        }
    }

    public void OnSelectItem(ShopItem item)
    {
        audioServiceSO.PlaySFX(audioSource,AudioType.ButtonClick);
            switch (item.carDataSO.vehicleData.vehicleStatus)
            {
                case VehicleStatus.Buy:
                //implement buy based on funds
                if(item.carDataSO.vehicleData.amount <= gameDataSO.coinAmount)
                {
                    GameService.Instance.mainMenuUIService.TweenCoinVisual(item.carDataSO.vehicleData.amount);
                    gameDataSO.coinAmount -= item.carDataSO.vehicleData.amount;
                    item.ToggleButtonText(VehicleStatus.Equip);
                    item.carDataSO.vehicleData.vehicleStatus = VehicleStatus.Equip;
                    audioServiceSO.PlaySFX(audioSource,AudioType.BuySuccess);
                    saveServiceSO.SaveData();
                }
                else
                {
                    //toggle not enough coins panel to watch ad
                    GameService.Instance.mainMenuUIService.OpenWatchAdPanel();
                }
                    break;
                case VehicleStatus.Equip:
                 foreach(ShopItem shopitem in shopItems)
                 {
                    if(shopitem.carDataSO.vehicleData.vehicleStatus == VehicleStatus.Equipped)
                    {
                        shopitem.carDataSO.vehicleData.vehicleStatus = VehicleStatus.Equip;
                        shopitem.ToggleButtonText(VehicleStatus.Equip);
                    }
                 }
                item.carDataSO.vehicleData.vehicleStatus = VehicleStatus.Equipped;
                item.ToggleButtonText(VehicleStatus.Equipped);
                gameDataSO.playerVehiclePrefab = item.carDataSO.vehicleData.vehiclePrefab;
                saveServiceSO.SaveData();
                break;
        }
      
    }
   

}
