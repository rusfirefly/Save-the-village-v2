
using UnityEngine;
using UnityEngine.UI;

public class GameMenu: MonoBehaviour
{
    [SerializeField] private Canvas _pauseMenu;
    public static bool isPaused;

    public static GameMenu menuInstance;

    private void Start()
    {
        if (menuInstance == null)
            menuInstance = FindAnyObjectByType<GameMenu>();
    }
    public void PauseMenu()
    {
        isPaused = !isPaused;
        _pauseMenu.gameObject.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
