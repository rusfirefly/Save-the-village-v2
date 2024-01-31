using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundSpawn : MonoBehaviour
{
    [SerializeField] public AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clip;
    private Random _random;

    private void Start()
    {
        _random = new Random();
    }

    public void PlaySound()
    {
        int index = _random.Next(_clip.Length);
        _audioSource.clip = _clip[index];
        _audioSource.Play();
    }

    private void OnValidate()
    {
        GetAudioSouceComponent();
    }

    private void GetAudioSouceComponent() => _audioSource ??= GetComponent<AudioSource>();
}
