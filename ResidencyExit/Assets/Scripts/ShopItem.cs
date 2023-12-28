using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField] Image carImage;
    [SerializeField] TMP_Text equippedText;
    [SerializeField] TMP_Text buyText;
    [SerializeField] TMP_Text equipText;
    public Button button;
    public VehicleDataScriptableObject carDataSO { get; set; }
   
   
    public void InitItem(VehicleDataScriptableObject carDataSO)
    {
        this.carDataSO = carDataSO;
        this.carImage.sprite = carDataSO.vehicleData.vehicleSprite;
        ToggleButtonText(carDataSO.vehicleData.vehicleStatus);
        
    }

    public void ToggleButtonText(VehicleStatus status)
    {
        equippedText.gameObject.SetActive(false);
        buyText.gameObject.SetActive(false);
        equipText.gameObject.SetActive(false);

        switch (status)
        {
            case VehicleStatus.Equip:
                equipText.gameObject.SetActive(true);
                break;
            case VehicleStatus.Equipped:
                equippedText.gameObject.SetActive(true);
                break;
            case VehicleStatus.Buy:
                buyText.gameObject.SetActive(true);
                buyText.text += carDataSO.vehicleData.amount.ToString();
                break;
        }

    }

}
