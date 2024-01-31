using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundBulding : MonoBehaviour
{
    [SerializeField] public AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;


    public void PlaySound()
    {
        _audioSource.Play();
    }

    private void OnValidate()
    {
        GetAudioSouceComponent();
        _audioSource.clip ??= _clip;
    }

    private void GetAudioSouceComponent() => _audioSource ??= GetComponent<AudioSource>();
}
