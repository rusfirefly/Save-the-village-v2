
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Enums;

public class GameMenu: MonoBehaviour
{
    [SerializeField] private Canvas _pauseMenu;
    [SerializeField] private Canvas _HUDMenu;
    [SerializeField] private Canvas _mainMenu;
    [SerializeField] private Canvas _hudGame;
    [SerializeField] private Canvas _gameOverCanvas;
    [SerializeField] private Text _statisticText;
    [SerializeField] private Text _titleGameOver;
    [SerializeField] private Tutorial _tutorial;

    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _victorySound;

    [SerializeField] private AudioClip _pauseSound;
    [SerializeField] private AudioClip _resumeSound;

    private SoundClip _soundClip;

    public static bool isPaused;
    private bool _isMainMenu;
    private bool _isSoundOff;
    public static GameMenu menuInstance;
    

    public void Initialize(Tutorial tutorial)
    {
        _tutorial = tutorial;
        _isSoundOff = true;
        InitMenu();
        isPaused = true;
        GetSoundClipComponent();
        VisibleCanvas(_HUDMenu, true);
        VisibleMainMenu(true);
    }

    private void GetSoundClipComponent() => _soundClip = gameObject.GetComponent<SoundClip>();

    private void InitMenu()
    {
       menuInstance ??= FindAnyObjectByType<GameMenu>();
    }

    public void Reload()
    {
        SetPauseGame(isPause: false);
        VisibleCanvas(_HUDMenu, false);
        VisibleCanvas(_gameOverCanvas, false);
    }

    private void Update()
    {
        if(_isMainMenu)
        {
            if (Input.anyKeyDown) 
            {
                VisibleCanvas(_HUDMenu, false);
                VisibleMainMenu(false);
                VisibleCanvas(_hudGame, true);

                _isMainMenu = false;
                isPaused = false;
            }
        }
    }

    public void PauseMenu(bool isPause)
    {
        SetPauseGame(isPause);
        PlaySound(isPause);
        VisibleCanvas(_HUDMenu, isShow: isPause);
        VisibleCanvas(_pauseMenu, isShow: isPause);
    }

    public void ShowGameOverMenu(string statisticText, GameOverType type)
    {
        SetTitleGameOver(type);
        PlaySound(type);
        SetStatisticText(statisticText);
        SetPauseGame(isPause: true);
        VisibleCanvas(_HUDMenu, true);
        VisibleCanvas(_gameOverCanvas, isShow: isPaused);
    }
    
    private void SetTitleGameOver(GameOverType type)
    {
        string title = "";
        switch (type)
        {
            case GameOverType.Lose:
                title= "GAME OVER";
                break;
            case GameOverType.Victory:
                title = "VICTORY";
                break;
        }
        _titleGameOver.text = title;
    }

    private void PlaySound(GameOverType type)
    {
        switch (type)
        {
            case GameOverType.Lose:
                _soundClip.PlaySound(_gameOverSound);
                break;
            case GameOverType.Victory:
                _soundClip.PlaySound(_victorySound);
                break;
        }
    }

    private void PlaySound(bool isPauseGame)
    {
        if (isPauseGame)
            _soundClip.PlaySound(_pauseSound);
        else
            _soundClip.PlaySound(_resumeSound);
    }

    public void ShowTutorial()
    {
        SetPauseGame(isPause: true);
        VisibleCanvas(_HUDMenu, true);
        _tutorial.Initialize();
    }

    public void ExitTutorial()
    {
        SetPauseGame(isPause: false);
        VisibleCanvas(_HUDMenu, false);
        _tutorial.ExitTutorial();
    }

    private void SetPauseGame(bool isPause)
    {
        isPaused = isPause;
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        PauseMenu(false);
    }

    private void VisibleMainMenu(bool isShow)
    {
        _isMainMenu = true;
        VisibleCanvas(_mainMenu, isShow);
    }

    public void SetSounOnOff(Text buttonText)
    {
        if (_isSoundOff)
        {
            SoundSystem.soundInstance.AllSoundOff();
            buttonText.text = "¬ключить\nзвук";
        }
        else
        {
                        
            SoundSystem.soundInstance.AllSoundOn();
            buttonText.text = "ќтключить\nзвук";
        }
        _isSoundOff = !_isSoundOff;
    }

    private void SetStatisticText(string text) => _statisticText.text = text;

    private void VisibleCanvas(Canvas canvas, bool isShow) => canvas.gameObject.SetActive(isShow);

}
