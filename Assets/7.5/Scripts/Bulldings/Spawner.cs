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
    private SoundSpawn _sound;
    [SerializeField] private GameObject _waveInfoCanvas;

    private float _currentTime;
    private float _timeSpawn;
    private int _countEnemy;
    private int _countEnemysInWave;
    private int _indexEnemyRand;
    private TimeSpan _time;
    private Random _random;
    private Random _randomEnemy;

    private bool _waveInfoVisible;
    private const int _xRandomMin = -5;
    private const int _xRandomMax = 6;
    private const float _xKoef = 0.1f;
    private const float _xOffset = 0.6f;

    private const int _yRandomMin = -2;
    private const int _yRandomMax = 3;
    private const float _yKoef = 0.1f;
    private const float _yOffset = 0.3f;

    private void Awake()
    {
        Initialized();
    }

    public void Initialized()
    {
        _randomEnemy = new Random();

        _playerData.numberWave = 1;
        _countEnemy = 1;

        _indexEnemyRand = _randomEnemy.Next(0, _countEnemysInWave);
        _countEnemysInWave = _prefabEnemys.Length;

        _countEnemysText.text = $"Врагов в следующем набеге: {_countEnemy}";
        _waveText.text = $"Волна: {_playerData.numberWave}";

        _sound = gameObject.GetComponent<SoundSpawn>();
        WaveCanvasVisible(true);
        _random = new Random();
    }

    public void Reload()
    {
        _countEnemy = 1;
        _countEnemysText.text = $"Врагов в следующем набеге: {_countEnemy}";
        _currentTime = 0;
        _playerData.numberWave = 1;
        _waveText.text = $"Волна: {_playerData.numberWave}";
    }

    private void Update()
    {
        UpdateSpawn();
    }

    public void UpdateSpawn()
    {
        if (GameMenu.isPaused) return;
        if (Storage.Gold <= _random.Next(50, 150)) return;


        _currentTime += Time.deltaTime;
        _timeSpawn = _playerData.waveCycleTime - _currentTime;

        _time = new TimeSpan(0, 0, Mathf.RoundToInt(_timeSpawn));
        _cycleWaveText.text = $"Время до набега врагов: {_time.Minutes} м {_time.Seconds} с";
            
        if (_currentTime>= _playerData.waveCycleTime)
        {
            for(int i = 0; i < _countEnemy; i++)
                SpawnEnemy(_countEnemy);

            _sound.PlaySound();
            _countEnemy = _random.Next(0, 3) + _playerData.numberWave;
            _playerData.numberWave++;
            _countEnemysText.text = $"Врагов в следующем набеге: {_countEnemy}";
            _waveText.text =$"Волна: {_playerData.numberWave}";
            _currentTime = 0;
        }
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
    private void SpawnEnemy(int countEnemy)
    {
        Vector3 newPoint = SetRandomPosition(_spawnPosition.position);
        Enemy enemy  = Instantiate(_prefabEnemys[_indexEnemyRand], newPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetTargetPosition(_targetPosition);
        enemy.InitEnemy();
        _countEnemysInWave = _prefabEnemys.Length;
        _indexEnemyRand = _randomEnemy.Next(0, _countEnemysInWave);
    }

    private Vector3 SetRandomPosition(Vector3 position)
    {
        return new Vector3(position.x + (_random.Next(_xRandomMin, _xRandomMax) + _xKoef) * _xOffset, position.y + (_random.Next(_yRandomMin, _yRandomMax) + _yKoef) * _yOffset);
    }
}
