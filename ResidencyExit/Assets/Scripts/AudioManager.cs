using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] SoundDataSO soundData;
    private AudioSource source;
    private WaitForSeconds waitTime;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        DontDestroyOnLoad(this);

    }

    public void PlayClick()
    {
        source.clip = soundData.GetClip(0);
        source.Play();
    }
    //public void PlaySound(int index)
    //{
    //    source.clip = soundData.GetClip(index);
    //    source.PlayOneShot(source.clip);
    //}
    //public void PlaySoundLoop(int index,float pitch = 1)
    //{
    //    source.pitch = pitch;
    //    source.loop = true;
    //    source.clip = soundData.GetClip(index);
    //    source.Play();

    //}

    //public void ResetAudioSource() 
    //{
    //    source.clip = null;
    //    source.pitch = 1;
    //    source.loop = false;
    //}

    //public void PlaySoundAt(AudioSource source,int index)
    //{
    //    source.clip = soundData.GetClip(index);
    //    source.PlayOneShot(source.clip);
    //}

   

}
