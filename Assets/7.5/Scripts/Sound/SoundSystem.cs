using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundSystem : MonoBehaviour
{
    public static SoundSystem soundInstance { get; private set; }
    private AudioSource[] _allAudio;
    private float[] volumes;

    public void Initialize()
    {
        soundInstance ??= FindAnyObjectByType<SoundSystem>();
        _allAudio = FindAllAudioSource();
        volumes = new float[_allAudio.Length];
    }
   
    public void AllSoundOff()
    {
        int audioID = 0;
        foreach (AudioSource audio in _allAudio)
        {
            SaveSoundVolume(audioID++, audio.volume);
            SetAudioVolume(audio, volume: 0);
        }
        Debug.Log($"звуки отключены. Кол-во источников: {_allAudio.Length}");
    }

    public void AllSoundOn()
    {
        _allAudio =  FindAllAudioSource();
        int audioID = 0;
        foreach (AudioSource audio in _allAudio)
        {
            audio.volume = GetSoundVolume(audioID);
        }
        Debug.Log($"звуки включины. Кол-во источников: {_allAudio.Length}");
    }

    private void SetAudioVolume(AudioSource audio, float volume) => audio.volume = 0;

    private void SaveSoundVolume(int audioID, float volume)=> volumes[audioID] = volume;

    private float GetSoundVolume(int audioID) => volumes[audioID];

    private AudioSource[] FindAllAudioSource()
    {
        return  FindObjectsOfType<AudioSource>();
    }
}
