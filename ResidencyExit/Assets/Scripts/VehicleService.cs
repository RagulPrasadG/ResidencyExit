using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleService 
{
    private GameDataSO gameDataSO;
    private EventServiceScriptableObject eventServiceSO;
    private AudioServiceScriptableObject audioServiceSO;

    public VehicleService(GameDataSO gameDataSO, EventServiceScriptableObject eventServiceSO, 
        AudioServiceScriptableObject audioServiceSO)
    {
        this.gameDataSO = gameDataSO;
        this.eventServiceSO = eventServiceSO;
        this.audioServiceSO = audioServiceSO;
    }

    public void CreateVehicle(Vector3 position ,CinemachineVirtualCamera followCamera)
    {
        VehicleController vehicleController = new VehicleController(gameDataSO,eventServiceSO,audioServiceSO);
        followCamera.Follow = vehicleController.vehicleView.transform;
        followCamera.LookAt = vehicleController.vehicleView.transform;
        vehicleController.SetPosition(position);
    }
}
