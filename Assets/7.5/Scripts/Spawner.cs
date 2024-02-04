using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    private float _currentTime;
    [SerializeField] private GameObject[] _prefabEnemys;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Text _cycleWaveText;
    [SerializeField] private Text _countEnemysText;
    [SerializeField] private SoundSpawn _sound;

    private int _countStekEnemy;
    private float _timeSpawn;
    private Random _randomEnemy;
    private int _countEnemysInWave;
    private int _indexEnemyRand;
    private TimeSpan _time;

    private void Awake()
    {
        _randomEnemy = new Random();
        _playerData.numberWave = 1;
        _countStekEnemy = 1;
        _countEnemysInWave = _prefabEnemys.Length;
        _indexEnemyRand = _randomEnemy.Next(0, _countEnemysInWave);
        _countEnemysText.text = $"������ � ��������� ������: {_countStekEnemy}";
        _sound = gameObject.GetComponent<SoundSpawn>();
    }

    public void Reload()
    {
        _countStekEnemy = 1;
        _countEnemysText.text = $"������ � ��������� ������: {_countStekEnemy}";
        _currentTime = 0;
    }

    private void Update()
    {
        UpdateSpawn();
    }

    public void UpdateSpawn()
    {
        if (GameMenu.isPaused) return;
        _currentTime += Time.deltaTime;
        _timeSpawn = _playerData.waveCycleTime - _currentTime;

        _time = new TimeSpan(0, 0, Mathf.RoundToInt(_timeSpawn));
        _cycleWaveText.text = $"����� �� ������ ������: {_time.Minutes} � {_time.Seconds} �";
            
        if (_currentTime>= _playerData.waveCycleTime)
        {
            for(int i = 0; i < _countStekEnemy; i++)
                SpawnEnemy(_countStekEnemy);

            _sound.PlaySound();
            _countStekEnemy++;
            _playerData.numberWave++;
            _countEnemysText.text = $"������ � ��������� ������: {_countStekEnemy} �����: {_playerData.numberWave}";
            _currentTime = 0;
        }
    }

    private void SpawnEnemy(int countEnemy)
    {
        Enemy enemy  = Instantiate(_prefabEnemys[_indexEnemyRand], _spawnPosition.position, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetTargetPosition(_targetPosition);
        enemy.InitEnemy(1);
        _countEnemysInWave = _prefabEnemys.Length;
        _indexEnemyRand = _randomEnemy.Next(0, _countEnemysInWave);
    }
}
