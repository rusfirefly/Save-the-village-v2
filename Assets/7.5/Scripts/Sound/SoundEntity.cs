using UnityEngine;
using Random = System.Random;

public class SoundEntity : Sound
{
    [SerializeField] private AudioClip[] _newEntitySound;
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _deathSound;
    private Random _random;
    protected int _indexSound;

    public void PlaySoundHit()
    {
        _audioSource.clip = _hitSound;
        _audioSource.Play();
    }

    public void PlaySoundAttack()
    {
        _audioSource.clip = _attackSound;
        _audioSource.Play();
    }

    public void PlaySoundDeath()
    {
        _audioSource.clip = _deathSound;
        _audioSource.Play();
    }

    public void PlaySoundNewEntity()
    {
        _random = new Random();
        _indexSound = _random.Next(_newEntitySound.Length);
        _audioSource.clip = _newEntitySound[_indexSound];
        _audioSource.Play();
    }

}
