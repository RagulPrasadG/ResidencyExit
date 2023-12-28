using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
        this.audioServiceSO.PlaySFX(vehicleView.source, AudioType.CarIgnition);
    }
    public void SetPosition(Vector3 position) => vehicleView.transform.position = position;

    public void MoveLeft()
    {
        if (!canMove) return;

        audioServiceSO.PlaySFX(vehicleView.source, AudioType.ButtonClick);
        vehicleView.animator.SetBool("isMoving", true);
        Vector3 endPosition = GridManager.instance.GetLeftEndPosition();
        if (endPosition == Vector3.zero)
        {
            vehicleView.animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        vehicleView.source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - vehicleView.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(vehicleView.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(vehicleView.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            vehicleView.source.DOPitch(1.0f, 0.2f);
            vehicleView.animator.SetBool("isMoving", false);
        };
    }

    public void MoveRight()
    {
        if (!canMove) return;

        audioServiceSO.PlaySFX(vehicleView.source, AudioType.ButtonClick);
        vehicleView.animator.SetBool("isMoving", true);
        
        Vector3 endPosition = GridManager.instance.GetRightEndPosition();
        if (endPosition == Vector3.zero)
        {
            vehicleView.animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        vehicleView.source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - vehicleView.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(vehicleView.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(vehicleView.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            vehicleView.source.DOPitch(1.0f, 0.2f);
            vehicleView.animator.SetBool("isMoving", false);

        };
    }

    public void MoveUp()
    {
        if (!canMove) return;

        audioServiceSO.PlaySFX(vehicleView.source, AudioType.ButtonClick);
        vehicleView.animator.SetBool("isMoving", true);
        Vector3 endPosition = GridManager.instance.GetUpEndPosition();
        if (endPosition == Vector3.zero)
        {
            vehicleView.animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        vehicleView.source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - vehicleView.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(vehicleView.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(vehicleView.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            vehicleView.source.DOPitch(1.0f, 0.2f);
            vehicleView.animator.SetBool("isMoving", false);
        };
    }
    public void MoveDown()
    {
        if (!canMove) return;

        audioServiceSO.PlaySFX(vehicleView.source, AudioType.ButtonClick);
        vehicleView.animator.SetBool("isMoving", true);
        Vector3 endPosition = GridManager.instance.GetDownEndPosition();
        if (endPosition == Vector3.zero)
        {
            vehicleView.animator.SetBool("isMoving", false);
            return;        //if the endposition is the same as car position dont move
        }
        vehicleView.source.DOPitch(1.1f, 0.2f);
        Vector3 direction = (endPosition - vehicleView.transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(vehicleView.transform.DORotateQuaternion(rotation, 1f));
        sequence.Append(vehicleView.transform.DOMove(endPosition, 2f));
        sequence.Play();
        canMove = false;
        sequence.onComplete += () =>
        {
            canMove = true;
            vehicleView.source.DOPitch(1.0f, 0.2f);
            vehicleView.animator.SetBool("isMoving", false);
        };
    }

}
