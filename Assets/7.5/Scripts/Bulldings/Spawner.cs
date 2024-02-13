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

    [SerializeField] private Text _cycleWaveText;
    [SerializeField] private Text _countEnemysText;
    [SerializeField] private Text _waveText;
    [SerializeField] private int _warriorPeakEnemyRaid;
    private SoundSpawn _sound;
    [SerializeField] private GameObject _waveInfoCanvas;

    private float _currentTime;
    private float _timeSpawn;
    private int _countEnemy;
    private int _indexEnemyRand;
    private TimeSpan _time;
    private Random _randomEnemy;
    private Random _randomPosition;

    private bool _waveInfoVisible;
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
    }

    public void Initialized()
    {
        GetSoundComponent();
        _randomEnemy = new Random();
        _randomPosition = new Random();
        Reload();
        _indexEnemyRand = GetRandomEnemy();
        UpdateEnemyInformation(_countEnemy);
        UpdateWaveInfo(_playerData.numberWave);
        WaveCanvasVisible(true);
    }

    private int GetRandomEnemy() => _randomEnemy.Next(0, _prefabEnemys.Length);
    private void GetSoundComponent() => _sound = gameObject.GetComponent<SoundSpawn>();
    public void Reload()
    {
        _countEnemy = 1;
        _currentTime = 0;
        _playerData.numberWave = 1;

        UpdateEnemyInformation(_countEnemy);
        UpdateWaveInfo(_playerData.numberWave);
    }

    private void Update()
    {
        UpdateSpawn();
    }

    public void UpdateSpawn()
    {
        if (GameMenu.isPaused) return;
        if ((Population.ArcherCount+Population.WarriorsCount) < _warriorPeakEnemyRaid) return;

        _currentTime += Time.deltaTime;
        _timeSpawn = _playerData.waveCycleTime - _currentTime;

        SetCycleWaveText(_timeSpawn);

        if (_currentTime>= _playerData.waveCycleTime)
        {
            for(int i = 0; i < _countEnemy; i++)
                SpawnEnemy();

            _sound.PlaySound();
            _countEnemy =  _playerData.numberWave;
            _playerData.numberWave++;

            UpdateEnemyInformation(_countEnemy);
            UpdateWaveInfo(_playerData.numberWave);

            _currentTime = 0;
        }
    }

    private void SetCycleWaveText(float timeSpawn)
    {
        _time = new TimeSpan(0, 0, Mathf.RoundToInt(_timeSpawn));
        _cycleWaveText.text = $"Время до набега врагов: {_time.Minutes} м {_time.Seconds} с";
    }

    private void WaveCanvasVisible(bool visible)
    {
        if(visible&& !_waveInfoVisible)
        {
            _waveInfoCanvas.transform.localPosition = new Vector3(_waveInfoCanvas.transform.localPosition.x, _waveInfoCanvas.transform.localPosition.y-198);
        }
        else
        {
            _waveInfoCanvas.transform.localPosition = new Vector3(_waveInfoCanvas.transform.localPosition.x, _waveInfoCanvas.transform.localPosition.y + 198);
        }
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

    private void UpdateEnemyInformation(int countEnemy)
    {
        _countEnemysText.text = $"Врагов в следующем набеге: {countEnemy}";
    }

    private void UpdateWaveInfo(int numberWave)
    {
        _waveText.text = $"Волна: {numberWave}";
    }
}
