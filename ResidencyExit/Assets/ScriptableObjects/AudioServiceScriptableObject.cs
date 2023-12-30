using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioService",menuName = "Data/NewAudioService")]
public class AudioServiceScriptableObject : ScriptableObject
{
    [SerializeField] SoundDataScriptableObject soundData;
    private WaitForSeconds waitTime;

    public void PlaySFX(AudioSource audioSource,AudioType audioType)
    {
        audioSource.loop = false;
        audioSource.clip = soundData.GetClip(audioType);
        audioSource.PlayOneShot(audioSource.clip);
    }

    public void PlaySoundLoop(AudioSource audioSource,AudioType audioType, float pitch = 1)
    {
        audioSource.pitch = pitch;
        audioSource.loop = true;
        audioSource.clip = soundData.GetClip(audioType);
        audioSource.Play();

    }

    public void ResetAudioSource(AudioSource audioSource)
    {
        audioSource.clip = null;
        audioSource.pitch = 1;
        audioSource.loop = false;
    }

    //public void PlaySoundAt(AudioSource source, AudioType audioType)
    //{
    //    source.clip = soundData.GetClip(audioType);
    //    source.PlayOneShot(source.clip);
    //}
}
