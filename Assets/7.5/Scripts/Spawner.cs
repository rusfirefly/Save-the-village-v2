using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private float _currentTime;
    [SerializeField] private GameObject _prefabEnemy;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private Text _cycleWaveText;
    [SerializeField] private Text _powerEnemysText;
    [SerializeField] private Text _countEnemysText;
    private int _countStekEnemy;
    private float _timeSpawn;
    private void Awake()
    {
        _playerData.numberWave = 1;
        _countStekEnemy = 1;
        _countEnemysText.text = $"Колво врагов в слудующем набеге: {_countStekEnemy}";
    }

    private void Update()
    {
        UpdateSpawn();
    }
    public void UpdateSpawn()
    {
        _currentTime += Time.deltaTime;
        _timeSpawn = _playerData.waveCycleTime - _currentTime;
        TimeSpan TS = new TimeSpan(0, 0, Mathf.RoundToInt(_timeSpawn));
        _cycleWaveText.text = $"Время до набега врагов: {TS.Minutes} м {TS.Seconds} с";
            
        if (_currentTime>= _playerData.waveCycleTime)
        {
            SpawnEnemy(_countStekEnemy);
            _countStekEnemy ++;
            _countEnemysText.text = $"Колво врагов в следующем набеге: {_countStekEnemy}";
            _playerData.numberWave++;
            
            _currentTime = 0;
        }
    }

    private void SpawnEnemy(int countEnemy)
    {
        Enemy enemy  = Instantiate(_prefabEnemy, _spawnPosition.position, Quaternion.identity).GetComponent<Enemy>();
        enemy.SetTargetPosition(_targetPosition);
        enemy.InitEnemy(1,2,2, countEnemy);

        _playerData.powerEnemys = enemy.GetPowerEnemy();
        _powerEnemysText.text = $"Сила врага: {_playerData.powerEnemys}";

        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        if (enemys.Length == 1)
            enemys[0].firstEnemy = true;

    }
}
