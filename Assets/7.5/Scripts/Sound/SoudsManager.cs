using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoudsManager : MonoBehaviour
{
    public static SoudsManager soundInstance { get; private set; }
    private AudioSource[] _allAudio;

    public void Start()
    {
        Init();
    }
   
    public void AllSoundOff()
    {
        _allAudio = FindAllAudioSource();
        foreach (AudioSource audio in _allAudio)
            audio.enabled = false;
        Debug.Log($"звуки отключены. Кол-во источников: {_allAudio.Length}");
    }

    public void AllSoundOn()
    {
        _allAudio =  FindAllAudioSource();
        foreach (AudioSource audio in _allAudio)
            audio.enabled = true;
        Debug.Log($"звуки включины. Кол-во источников: {_allAudio.Length}");
    }

    private AudioSource[] FindAllAudioSource()
    {
        return GameObject.FindObjectsOfType<AudioSource>();
    }

    private void Init()
    {
        soundInstance ??= FindAnyObjectByType<SoudsManager>();
    }
}
