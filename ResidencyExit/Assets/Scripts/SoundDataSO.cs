using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSoundData",menuName = "Data/NewSoundData")]
public class SoundDataSO : ScriptableObject
{
    public List<AudioClip> audioClips;

    public AudioClip GetClip(int index)
    {
        return audioClips[index];
    }

}
