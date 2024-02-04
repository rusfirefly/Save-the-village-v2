using UnityEngine;

public class SoundClip : Sound
{
    [SerializeField] private AudioClip _clip;

    public void PlaySound()
    {
        _audioSource.volume = volume;
        _audioSource.clip = _clip;
        _audioSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.volume = volume;
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
