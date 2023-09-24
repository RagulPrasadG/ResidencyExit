using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCarData",menuName = "Data/NewCarData")]
public class CarDataSO : ScriptableObject
{
    public CarData carData;
}

[System.Serializable]
public enum CarStatus
{
    Equip,Equipped,Buy
}


[System.Serializable]
public class CarData
{
    public Sprite carSprite;
    public GameObject carPrefab;
    public int amount;
    public CarStatus carStatus;
}
