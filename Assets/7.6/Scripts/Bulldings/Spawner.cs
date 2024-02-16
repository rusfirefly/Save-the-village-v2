using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabEnemys;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private GameSetup _gameSetup;

    [SerializeField] private HUDGame _hudGame;

    [SerializeField] private GameObject _waveInfoCanvas;
   
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
        GameHadler.ReloadAll += OnReloadAll;
        DayAndNight.NewDay += OnNewDay;
    }
    
    private void Update()
    {
        UpdateSpawn();
    }

    private void OnDestroy()
    {
        DayAndNight.NewDay -= OnNewDay;
        GameHadler.ReloadAll -= OnReloadAll;
    }

    public void Initialized()
    {
        GetSoundComponent();
        Reload();
        _randomEnemy = new Random();
        _randomPosition = new Random();
        _indexEnemyRand = GetRandomEnemy();
        _hudGame.UpdateEnemyInformation(_countEnemy);
        _hudGame.UpdateWaveInfo(_gameSetup.numberWave);
        _hudGame.UpdateDayToDeadline(_gameSetup.deadlineDay);
    }

    public void Reload()
    {
        SetDefaultValue();

        WaveCanvasVisible(false);
        _hudGame.UpdateDayToDeadline(_gameSetup.deadlineDay - _currentDay);
        _hudGame.UpdateNumberDay(_currentDay);
        _hudGame.UpdateEnemyInformation(_countEnemy);
        _hudGame.UpdateWaveInfo(_gameSetup.numberWave);
    }

    public void UpdateSpawn()
    {
        if (GameMenu.isPaused) return;
        if (_currentDay < _gameSetup.deadlineDay) return;

        _currentTime += Time.deltaTime;
        _timeSpawn = _gameSetup.waveCycleTime - _currentTime;
        _hudGame.SetCycleWaveText(_timeSpawn);

        if (_currentTime >= _gameSetup.waveCycleTime)
        {
            for (int i = 0; i < _countEnemy; i++)
                SpawnEnemy();

            PlayeSoundAppearedEnemy();
            _countEnemy++;
            _gameSetup.numberWave++;

            _hudGame.UpdateEnemyInformation(_countEnemy);
            _hudGame.UpdateWaveInfo(_gameSetup.numberWave);

            _currentTime = 0;
        }
    }

    private void PlayeSoundAppearedEnemy()=>_sound.PlaySound();

    private void OnReloadAll()
    {
        Reload();
    }

    private void SetDefaultValue()
    {
        _countEnemy = 1;
        _currentTime = 0;
        _gameSetup.numberWave = 1;
        _currentDay = 1;
    }

    private int GetRandomEnemy() => _randomEnemy.Next(0, _prefabEnemys.Length);

    private void GetSoundComponent() => _sound = gameObject.GetComponent<SoundSpawn>();

    private void OnNewDay(int currentDay)
    {
        _currentDay = currentDay;
        if (_currentDay == _gameSetup.deadlineDay)
        {
            WaveCanvasVisible(true);
            _hudGame.HideTextDayToDeadline();
        }

        if(_currentDay <= _gameSetup.deadlineDay)
            _hudGame.UpdateDayToDeadline(_gameSetup.deadlineDay - _currentDay);

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
