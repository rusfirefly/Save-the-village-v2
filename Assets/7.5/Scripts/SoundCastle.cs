using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundCastle : MonoBehaviour
{
    [SerializeField] public AudioSource _audioSource;
    [SerializeField] private AudioClip _clipCastleFire;
    [SerializeField] private AudioClip _clipCastleRepair;
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


    private void SetAudioClip(AudioClip clip) => _audioSource.clip = clip;

    private void OnValidate()
    {
        GetAudioSouceComponent();
    }

    private void GetAudioSouceComponent() => _audioSource ??= GetComponent<AudioSource>();
}
