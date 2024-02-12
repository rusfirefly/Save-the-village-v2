using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] [Range(0, 1)] protected float volume = 0.3f;  

    private void OnValidate()
    {
        GetAudioSouceComponent();
    }

    private void GetAudioSouceComponent() => _audioSource ??= GetComponent<AudioSource>();
}
