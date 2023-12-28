using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleView : MonoBehaviour
{
    public VehicleController vehicleController;
    public Animator animator;
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void SetController(VehicleController vehicleController) => this.vehicleController = vehicleController;

    private void OnTriggerEnter(Collider other)
    {
      
        if(other.gameObject.CompareTag("Coin"))
        {
            vehicleController.audioServiceSO.PlaySFX(audioSource, AudioType.CoinCollect);
            vehicleController.eventServiceSO.OnCollectCoin.RaiseEvent();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("GoalRing"))
        {
            vehicleController.eventServiceSO.OnReachGoal.RaiseEvent();
            Destroy(other.gameObject, 1f);
        }

       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("AICar"))
        {
            GameService.Instance.OnCarCrash();
        }
    }


}
