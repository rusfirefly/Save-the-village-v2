using System;
using System.Collections;
using System.Collections.Generic;
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
    private int _countStekEnemy;
    private int _countEnemysInWave;
    private int _indexEnemyRand;
    private TimeSpan _time;
    private Random _random;
    private Random _randomEnemy;

    private void Awake()
    {
        Initialized();
    }

    public void Initialized()
    {
        _randomEnemy = new Random();
        _playerData.numberWave = 1;
        _countStekEnemy = 1;
        _indexEnemyRand = _randomEnemy.Next(0, _countEnemysInWave);
        _countEnemysInWave = _prefabEnemys.Length;
        _countEnemysText.text = $"Врагов в следующем набеге: {_countStekEnemy}";
        _waveText.text = $"Волна: {_playerData.numberWave}";
        _sound = gameObject.GetComponent<SoundSpawn>();
        WaveCanvasVisible(true);
        _random = new Random();
    }

    public void Reload()
    {
        _countStekEnemy = 1;
        _countEnemysText.text = $"Врагов в следующем набеге: {_countStekEnemy}";
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
        if (Storage.Gold <= 50) return;

        _currentTime += Time.deltaTime;
        _timeSpawn = _playerData.waveCycleTime - _currentTime;

        _time = new TimeSpan(0, 0, Mathf.RoundToInt(_timeSpawn));
        _cycleWaveText.text = $"Время до набега врагов: {_time.Minutes} м {_time.Seconds} с";
            
        if (_currentTime>= _playerData.waveCycleTime)
        {
            for(int i = 0; i < _countStekEnemy; i++)
                SpawnEnemy(_countStekEnemy);

            _sound.PlaySound();
            _countStekEnemy++;
            _playerData.numberWave++;
            _countEnemysText.text = $"Врагов в следующем набеге: {_countStekEnemy}";
            _waveText.text =$"Волна: {_playerData.numberWave}";
            _currentTime = 0;
        }
    }
    private void WaveCanvasVisible(bool visible)
    {
        if(visible)
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
        Vector3 newPoint = new Vector3(_spawnPosition.position.x + (_random.Next(-5, 6) + 0.1f) * 0.6f, _spawnPosition.position.y + (_random.Next(-2, 3) + 0.1f) * 0.3f);
        Enemy enemy  = Instantiate(_prefabEnemys[_indexEnemyRand], newPoint, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetTargetPosition(_targetPosition);
        enemy.InitEnemy();
        _countEnemysInWave = _prefabEnemys.Length;
        _indexEnemyRand = _randomEnemy.Next(0, _countEnemysInWave);
    }
}
