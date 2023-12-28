using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVehicleData",menuName = "Data/NewVehicleData")]
public class VehicleDataScriptableObject : ScriptableObject
{
    public VehicleData vehicleData;
}

[System.Serializable]
public enum VehicleStatus
{
    Equip,
    Equipped,
    Buy
}


[System.Serializable]
public class VehicleData
{
    public Sprite vehicleSprite;
    public VehicleView vehiclePrefab;
    public int amount;
    public VehicleStatus vehicleStatus;
}
