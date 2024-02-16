using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Tutorial _tutorial;
    [SerializeField] private SoundSystem _soundSystem;
    [SerializeField] private HUDGame _hudSystem;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private GameMenu _gameMenuSystem;
    [SerializeField] private DayAndNight _dayNight;

    private void Start()
    {
        _gameManager.Initialize();
        _soundSystem.Initialize();
        _hudSystem.Initialize();
        _gameMenuSystem.Initialize(_tutorial);
        _dayNight.Initialize();
    }
}
