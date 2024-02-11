
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] private PlayerData _playerData;

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
        VisibleCanvas(_HUDMenu, true);
        VisibleMainMenu(true);
    }

    private void InitMenu()
    {
       menuInstance ??= FindAnyObjectByType<GameMenu>();
    }

    public void Reload()
    {
        ChangeGameTimeState();
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

    public void PauseMenu()
    {
        ChangeGameTimeState();
        VisibleCanvas(_HUDMenu, isShow:isPaused);
        VisibleCanvas(_pauseMenu, isShow:isPaused);
    }

    public void ShowGameOverMenu(string statisticText, string gameOverTitileText)
    {
        SetStatisticText(statisticText);
        _titleGameOver.text = gameOverTitileText;
        ChangeGameTimeState();
        VisibleCanvas(_HUDMenu, true);
        VisibleCanvas(_gameOverCanvas, isShow: isPaused);
    }

    public void ShowMainMenu()
    {

    }

    public void ShowTutorial()
    {
        ChangeGameTimeState();
        VisibleCanvas(_HUDMenu, true);
        _tutorial.Initialize();
    }

    public void ExitTutorial()
    {
        ChangeGameTimeState();
        VisibleCanvas(_HUDMenu, false);
        _tutorial.ExitTutorial();
    }

    private void ChangeGameTimeState()
    {
        isPaused = !isPaused;
        if (isPaused)
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
        PauseMenu();
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
            buttonText.text = "��������\n����";
        }
        else
        {
            SoundSystem.soundInstance.AllSoundOn();
            buttonText.text = "���������\n����";
        }
        _isSoundOff = !_isSoundOff;
    }

    private void SetStatisticText(string text) => _statisticText.text = text;

    private void VisibleCanvas(Canvas canvas, bool isShow) => canvas.gameObject.SetActive(isShow);

}