using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    [SerializeField] AudioServiceScriptableObject audioServiceSO;
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
      
        if(other.gameObject.CompareTag("Coin"))
        {
            audioServiceSO.PlaySFX(source, AudioType.CoinCollect);
            GameService.Instance.OnCoinCollect();
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("GoalRing"))
        {

            GameService.Instance.OnGoalReached();
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
