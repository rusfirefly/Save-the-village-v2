using System;
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
        int index = GetIndexLesson(_currentPage);
        SetVisibleButtons(index);
        SetPageText(index);
        LoadLeson(index);
    }

    public void Next()
    {
        int index = GetIndexLesson(++_currentPage);
        Debug.Log(index);
        SetVisibleButtons(index);
        SetPageText(index);
        LoadLeson(index);
    }

    public void Previous()
    {
        int index = GetIndexLesson(--_currentPage);
        Debug.Log(index);
        SetVisibleButtons(index);
        SetPageText(index);
        LoadLeson(index);
    }

    private int GetIndexLesson(int currentPage)
    {
        int index = Math.Clamp(currentPage, 0, _lesons.Length - 1);
        return index;
    }
    
    private void SetVisibleButtons(int index)
    {
        if (index > 0 && index < _lesons.Length)
        {
            ButtonPageActive(_prevPageButton, true);
            ButtonPageActive(_nextPageButton, true);
        }

        if (index == 0)
        {
            ButtonPageActive(_prevPageButton, false);
            ButtonPageActive(_nextPageButton, true);
        }

        if (index == _lesons.Length - 1)
        {
            ButtonPageActive(_prevPageButton, true);
            ButtonPageActive(_nextPageButton, false);
        }
    }

    public void ExitTutorial()
    {
        _hudBlur.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void GetVideoPlayer() => _videoPlayer ??= gameObject.GetComponent<VideoPlayer>();

    private void SetPageText(int page) => _pages.text = $"{page+1}/{_lesons.Length}";

    private void LoadLeson(int index)
    {
        _textLesson.text = _lesons[index].text;
        PlayVideoClip(_lesons[index].videoClip);
    }

    private void ButtonPageActive(Button button, bool active)=> button.gameObject.SetActive(active);

    private void OnValidate()
    {
        GetVideoPlayer();
    }

    private void PlayVideoClip(VideoClip videoClip)
    {
        _videoPlayer.clip = videoClip;
        _videoPlayer.Play();
    }
}
