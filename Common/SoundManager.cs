using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Enum.SoundType;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [SerializeField] private AudioSource _sourceBGM;
    [SerializeField] private AudioSource _sourceSE;

    public void PlayBGM(BGMType bgmType)
    {
        AudioClip clip = GetAudioClip(bgmType);
        if (_sourceBGM.clip == clip) return;
        _sourceBGM.clip = clip;
        _sourceBGM.Play();
    }

    public void PlaySE(BGMType bgmType) => _sourceSE.PlayOneShot(GetAudioClip(bgmType));

    public AudioClip GetAudioClip(BGMType bgmType) => GameDataManager.GetSoundData(bgmType);
}
