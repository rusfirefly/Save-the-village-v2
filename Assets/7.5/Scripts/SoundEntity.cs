using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEntity : MonoBehaviour
{
    [SerializeField] public AudioSource _audioSource;
    
    [SerializeField] private AudioClip _hit;
    [SerializeField] private AudioClip _attack;
    [SerializeField] private AudioClip _death;

    public void PlaySoundHit()
    {
        _audioSource.clip = _hit;
        _audioSource.Play();
    }

    public void PlaySoundAttack()
    {
        _audioSource.clip = _attack;
        _audioSource.Play();
    }

    public void PlaySoundDeath()
    {
        _audioSource.clip = _death;
        _audioSource.Play();
    }

    private void OnValidate()
    {
        GetAudioSouceComponent();
    }

    private void GetAudioSouceComponent() => _audioSource ??= GetComponent<AudioSource>();
}
