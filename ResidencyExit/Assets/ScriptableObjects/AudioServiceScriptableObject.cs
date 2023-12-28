using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioService",menuName = "Data/NewAudioService")]
public class AudioServiceScriptableObject : ScriptableObject
{
    [SerializeField] SoundDataSO soundData;
    private WaitForSeconds waitTime;

    private AudioSource audioSource;

    public void PlaySFX(AudioSource audioSource,AudioType audioType)
    {
        this.audioSource = audioSource;
        audioSource.loop = false;
        audioSource.clip = soundData.GetClip(audioType);
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlaySoundLoop(AudioSource audioSource,AudioType audioType, float pitch = 1)
    {
        this.audioSource = audioSource;
        audioSource.pitch = pitch;
        audioSource.loop = true;
        audioSource.clip = soundData.GetClip(audioType);
        audioSource.Play();

    }

    public void ResetAudioSource()
    {
        this.audioSource.clip = null;
        this.audioSource.pitch = 1;
        this.audioSource.loop = false;
    }

    //public void PlaySoundAt(AudioSource source, AudioType audioType)
    //{
    //    source.clip = soundData.GetClip(audioType);
    //    source.PlayOneShot(source.clip);
    //}
}
