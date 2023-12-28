using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleView : MonoBehaviour
{
    public VehicleController vehicleController;
    public Animator animator { get; set; }
    public AudioSource source { get; set; }

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if(other.gameObject.CompareTag("Coin"))
        {
            vehicleController.audioServiceSO.PlaySFX(source, AudioType.CoinCollect);
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
