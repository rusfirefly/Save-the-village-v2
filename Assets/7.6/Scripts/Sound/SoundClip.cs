using UnityEngine;

public class SoundClip : Sound
{
    public void PlaySound(AudioClip clip)
    {
        _audioSource.volume = volume;
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
