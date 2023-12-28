using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class VehicleController
{
    public EventServiceScriptableObject eventServiceSO;
    public AudioServiceScriptableObject audioServiceSO;
    public GameDataSO gameDataSO;

    public VehicleView vehicleView;
    private bool canMove = true;

    public VehicleController(GameDataSO gameDataSO, 
        EventServiceScriptableObject eventServiceSO,
        AudioServiceScriptableObject audioServiceSO)
    {
        this.eventServiceSO = eventServiceSO;
        this.audioServiceSO = audioServiceSO;
        this.gameDataSO = gameDataSO;
        this.vehicleView = Object.Instantiate(gameDataSO.playerVehiclePrefab);
        this.vehicleView.SetController(this);
        this.audioServiceSO.PlaySFX(vehicleView.audioSource, AudioType.CarIgnition);
        SetEvents();
    }

    public void SetEvents()
    {
        eventServiceSO.OnMoveVehicle.RemoveAllListeners();
        eventServiceSO.OnMoveVehicle.AddListener(MoveVehicle);
    }

    public void SetFollowCamera(CinemachineVirtualCamera followCamera)
    {
        followCamera.Follow = vehicleView.transform;
        followCamera.LookAt = vehicleView.transform;
    }

    public void SetPosition(Vector3 position) => vehicleView.transform.position = position;

    public void MoveVehicle(VehicleMoveDirection vehicleMoveDirection)
    {
        if (!canMove) return;

        audioServiceSO.PlaySFX(vehicleView.audioSource, AudioType.ButtonClick);
        vehicleView.animator.SetBool("isMoving", true);

        Vector3 destination = Vector3.zero;
        switch (vehicleMoveDirection)
        {
            case VehicleMoveDirection.Left:
                destination = GridManager.instance.GetLeftEndPosition();
                break;
            case VehicleMoveDirection.Right:
                destination = GridManager.instance.GetRightEndPosition();
                break;
            case VehicleMoveDirection.Down:
                destination = GridManager.instance.GetDownEndPosition();
                break;
            case VehicleMoveDirection.Up:
                destination = GridManager.instance.GetUpEndPosition();
                break;
        }

        if (destination == Vector3.zero)
        {
            vehicleView.animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        vehicleView.audioSource.DOPitch(1.1f, 0.2f);
        Vector3 direction = (destination - vehicleView.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(vehicleView.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(vehicleView.transform.DOMove(destination, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            vehicleView.audioSource.DOPitch(1.0f, 0.2f);
            vehicleView.animator.SetBool("isMoving", false);

        };
    }

}

public enum VehicleMoveDirection
{
    Up,Down,Left,Right
}

