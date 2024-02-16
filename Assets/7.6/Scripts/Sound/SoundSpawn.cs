using UnityEngine;
using Random = System.Random;

public class SoundSpawn : Sound
{
    [SerializeField] private AudioClip[] _clip;
    private Random _random;

    private void Awake()
    {
        _random = new Random();
    }
    
    public void PlaySound()
    {
        int index = _random.Next(_clip.Length);
        _audioSource.clip = _clip[index];
        _audioSource.Play();
    }
}
