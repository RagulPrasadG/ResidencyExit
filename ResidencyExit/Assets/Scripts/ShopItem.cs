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
    public CarDataSO carDataSO { get; set; }
   
   
    public void InitItem(CarDataSO carDataSO)
    {
        this.carDataSO = carDataSO;
        this.carImage.sprite = carDataSO.carData.carSprite;
        ToggleButtonText(carDataSO.carData.carStatus);
        
    }

    public void ToggleButtonText(CarStatus status)
    {
        equippedText.gameObject.SetActive(false);
        buyText.gameObject.SetActive(false);
        equipText.gameObject.SetActive(false);

        switch (status)
        {
            case CarStatus.Equip:
                equipText.gameObject.SetActive(true);
                break;
            case CarStatus.Equipped:
                equippedText.gameObject.SetActive(true);
                break;
            case CarStatus.Buy:
                buyText.gameObject.SetActive(true);
                buyText.text += carDataSO.carData.amount.ToString();
                break;
        }

    }

}
