using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabEnemys;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private HUDGame _hudGame;

    [SerializeField] private GameObject _waveInfoCanvas;
    [Header("Кол-во дней до рейда врагов")]
    [SerializeField] private int _deadlineDay;
    
    private SoundSpawn _sound;
    private int _currentDay;

    private float _currentTime;
    private float _timeSpawn;
    private int _countEnemy;
    private int _indexEnemyRand;
    private Random _randomEnemy;
    private Random _randomPosition;

    private const int _xRandomMin = -2;
    private const int _xRandomMax = 2;
    private const float _xKoef = 0.1f;
    private const float _xOffset = 0.6f;

    private const int _yRandomMin = -1;
    private const int _yRandomMax = 2;
    private const float _yKoef = 0.1f;
    private const float _yOffset = 0.3f;

    private void Awake()
    {
        Initialized();
        WaveCanvasVisible(true);
        GameManager.ReloadAll += OnReloadAll;
    }
    
    private void Update()
    {
        UpdateSpawn();
    }

    private void OnDestroy()
    {
        DayAndNight.NewDay -= OnNewDay;
        GameManager.ReloadAll -= OnReloadAll;
    }

    public void Initialized()
    {
        GetSoundComponent();
        _randomEnemy = new Random();
        _randomPosition = new Random();
        Reload();
        _indexEnemyRand = GetRandomEnemy();
        _hudGame.UpdateEnemyInformation(_countEnemy);
        _hudGame.UpdateWaveInfo(_playerData.numberWave);
        _hudGame.UpdateDayToDeadline(_deadlineDay);
        DayAndNight.NewDay += OnNewDay;
    }

    public void Reload()
    {
        _countEnemy = 1;
        _currentTime = 0;
        _playerData.numberWave = 1;
        _currentDay = 1;

        WaveCanvasVisible(false);
        _hudGame.UpdateDayToDeadline(_deadlineDay - _currentDay);
        _hudGame.UpdateNumberDay(_currentDay);
        _hudGame.UpdateEnemyInformation(_countEnemy);
        _hudGame.UpdateWaveInfo(_playerData.numberWave);
    }

    public void UpdateSpawn()
    {
        if (GameMenu.isPaused) return;
        if (_currentDay <= _deadlineDay) return;

        _currentTime += Time.deltaTime;
        _timeSpawn = _playerData.waveCycleTime - _currentTime;

        _hudGame.SetCycleWaveText(_timeSpawn);

        if (_currentTime >= _playerData.waveCycleTime)
        {
            for (int i = 0; i < _countEnemy; i++)
                SpawnEnemy();

            _sound.PlaySound();
            _countEnemy = _playerData.numberWave;
            _playerData.numberWave++;

            _hudGame.UpdateEnemyInformation(_countEnemy);
            _hudGame.UpdateWaveInfo(_playerData.numberWave);

            _currentTime = 0;
        }
    }

    private void OnReloadAll()
    {
        Reload();
    }

    private int GetRandomEnemy() => _randomEnemy.Next(0, _prefabEnemys.Length);

    private void GetSoundComponent() => _sound = gameObject.GetComponent<SoundSpawn>();

    private void OnNewDay(int currentDay)
    {
        _currentDay = currentDay;
        if (_currentDay == _deadlineDay)
        {
            WaveCanvasVisible(true);
            _hudGame.HideTextDayToDeadline();
        }

        if(_currentDay <= _deadlineDay)
            _hudGame.UpdateDayToDeadline(_deadlineDay - _currentDay);

        _currentDay++;
        _hudGame.UpdateNumberDay(_currentDay);
    }

    private void WaveCanvasVisible(bool visible)
    {
        if(visible)
        {
            _waveInfoCanvas.transform.localPosition = SetNewPositionCanvasInfoWave(newPositionY: -198);
        }
        else
        {
            _waveInfoCanvas.transform.localPosition = SetNewPositionCanvasInfoWave(newPositionY: 198);
        }
    }

    private Vector3 SetNewPositionCanvasInfoWave(float newPositionY)
    {
        return new Vector3(_waveInfoCanvas.transform.localPosition.x, _waveInfoCanvas.transform.localPosition.y + newPositionY);
    }

    private void SpawnEnemy()
    {
        Vector3 newPoint = SetRandomPosition(_spawnPosition.position);
        Enemy enemy  = Instantiate(_prefabEnemys[_indexEnemyRand], newPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetTargetPosition(_targetPosition);
        enemy.InitEnemy();
        _indexEnemyRand = GetRandomEnemy();
    }

    private Vector3 SetRandomPosition(Vector3 position)
    {
        return new Vector3(position.x + (_randomPosition.Next(_xRandomMin, _xRandomMax) + _xKoef) * _xOffset, 
                           position.y + (_randomPosition.Next(_yRandomMin, _yRandomMax) + _yKoef) * _yOffset);
    }
}
