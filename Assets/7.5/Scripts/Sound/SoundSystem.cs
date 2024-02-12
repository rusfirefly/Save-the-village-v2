using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    public static SoundSystem soundInstance { get; private set; }
    private AudioSource[] _allAudio;
    
    public void Initialize()
    {
        soundInstance ??= FindAnyObjectByType<SoundSystem>();
    }
   
    public void AllSoundOff()
    {
        _allAudio = FindAllAudioSource();
        foreach (AudioSource audio in _allAudio)
            audio.enabled = false;
        Debug.Log($"����� ���������. ���-�� ����������: {_allAudio.Length}");
    }

    public void AllSoundOn()
    {
        _allAudio =  FindAllAudioSource();
        foreach (AudioSource audio in _allAudio)
            audio.enabled = true;
        Debug.Log($"����� ��������. ���-�� ����������: {_allAudio.Length}");
    }

    private AudioSource[] FindAllAudioSource()
    {
        return  FindObjectsOfType<AudioSource>();
    }
}
