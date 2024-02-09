using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public struct Leson
{
    public VideoClip videoClip;
    [TextArea(14, 13)] public string text;
}

[RequireComponent(typeof(VideoPlayer))]

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Canvas _hudBlur;
    [SerializeField] private Button _prevPageButton;
    [SerializeField] private Button _nextPageButton;
    [SerializeField] private Button _exitButton;
    [SerializeField] private Text _textLesson;

    [SerializeField] private Text _pages;

    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private Leson[] _lesons;
    private int _currentPage;

    public void Initialize()
    {
        gameObject.SetActive(true);
        _prevPageButton.gameObject.SetActive(false);
        _currentPage = 0;
        _hudBlur.gameObject.SetActive(true);
        PageText(_currentPage);
        SelectPage(_currentPage);
    }

    public void Next()
    {
        _currentPage++;
        SelectPage(_currentPage);
    }
    public void Previous()
    {
        _currentPage--;
        SelectPage(_currentPage);
    }
    private void SelectPage(int currentPage)
    {
        int page = Math.Clamp(currentPage, 0, _lesons.Length - 1);
        PageText(page);

        if(page > 0 && page < _lesons.Length)
        {
            PageActive(_prevPageButton, true);
            PageActive(_nextPageButton, true);
        }

        if (page == 0)
        {
            PageActive(_prevPageButton, false);
            PageActive(_nextPageButton, true);
        }
        if (page == _lesons.Length - 1)
        {
            PageActive(_prevPageButton, true);
            PageActive(_nextPageButton, false);
        }

        LoadLeson(page);
        _currentPage = page;
    }
    
    public void ExitTutorial()
    {
        _hudBlur.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void PageText(int page) => _pages.text = $"{page+1}/{_lesons.Length}";
    private void LoadLeson(int index)
    {
        _textLesson.text = _lesons[index].text;
        PlayVideoClip(_lesons[index].videoClip);
    }

    private void PageActive(Button button, bool active)=> button.gameObject.SetActive(active);

    private void OnValidate()
    {
        GetVideoPlayer();
    }

    private void GetVideoPlayer() => _videoPlayer ??= gameObject.GetComponent<VideoPlayer>();
    private void PlayVideoClip(VideoClip videoClip)
    {
        _videoPlayer.clip = videoClip;
        _videoPlayer.Play();
    }
}
