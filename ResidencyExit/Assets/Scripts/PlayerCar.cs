using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if(other.gameObject.CompareTag("Coin"))
        {
            AudioManager.instance.PlaySoundAt(source, 1);
            GameService.instance.OnCoinCollect();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("GoalRing"))
        {

            GameService.instance.OnGoalReached();
            Destroy(other.gameObject, 1f);
        }

       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("AICar"))
        {
            GameService.instance.OnCarCrash();
        }
    }


}
