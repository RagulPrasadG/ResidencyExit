using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSoundData",menuName = "Data/NewSoundData")]
public class SoundDataSO : ScriptableObject
{
    public List<SoundData> audioClips;

    public AudioClip GetClip(AudioType audioType)
    {
        return audioClips.Find(audioData => audioData.audioType == audioType).audioClip;
    }

}

[System.Serializable]
public struct SoundData
{
    public AudioClip audioClip;
    public AudioType audioType;
}

public enum AudioType
{
    ButtonClick,
    CoinCollect,
    LevelUp,
    BuySuccess,
    CarCrash,
    CarIgnition
}
