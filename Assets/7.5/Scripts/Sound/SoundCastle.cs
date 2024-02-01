using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCastle : Sound
{
    [SerializeField] private AudioClip _clipCastleFire;
    [SerializeField] private AudioClip _clipCastleRepair;
    [SerializeField] private AudioClip _clipCastleDestroyed;
    [SerializeField] private AudioClip _clipWarning;
    [SerializeField] private AudioClip _clipNeedWood;
    

    public void PlaySoundCastleInFire()
    {
        SetAudioClip(_clipCastleFire);
        _audioSource.Play();
    }

    public void PlayeSoundCastleRepair()
    {
        SetAudioClip(_clipCastleRepair);
        _audioSource.Play();
    }

    public void PlayeSoundWarning()
    {
        SetAudioClip(_clipWarning);
        _audioSource.Play();
    }

    public void PlayeSoundNeedWood()
    {
        SetAudioClip(_clipNeedWood);
        _audioSource.Play();
    }

    public void PlayeSoundDestroyed()
    {
        SetAudioClip(_clipCastleDestroyed);
        _audioSource.Play();
    }

    private void SetAudioClip(AudioClip clip) => _audioSource.clip = clip;

}
