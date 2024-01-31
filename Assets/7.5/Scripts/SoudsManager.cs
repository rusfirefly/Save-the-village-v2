using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoudsManager : MonoBehaviour
{
    public static SoudsManager soundInstance { get; private set; }
    [SerializeField] private AudioSource _audioSorceBackgroundMusic;
    [SerializeField] private AudioClip[] _backgrounMusic;
    private AudioSource[] _allAudio;

    public void Start()
    {
        Init();
        
    }

    private void OnValidate()
    {
        GetAudioSouceComponent();
    }
    
    private void GetAudioSouceComponent()=> _audioSorceBackgroundMusic ??= GetComponent<AudioSource>();

    private void Init()
    {
        soundInstance ??= FindAnyObjectByType<SoudsManager>();
    }

    public void AllSoundOff()
    {
        _allAudio = FindAllAudioSource();
        foreach (AudioSource audio in _allAudio)
            audio.enabled = false;
        Debug.Log($"звуки отключены {_allAudio.Length}");
    }
    public void AllSoundOn()
    {
        _allAudio =  FindAllAudioSource();
        foreach (AudioSource audio in _allAudio)
            audio.enabled = true;
        Debug.Log($"звуки включины {_allAudio.Length}");
    }

    private AudioSource[] FindAllAudioSource()
    {
        return GameObject.FindObjectsOfType<AudioSource>();
    }

}
