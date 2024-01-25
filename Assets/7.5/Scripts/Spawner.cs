using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float _spawnTimer;
    private float _currentTime;
    [SerializeField] private GameObject _prefabEnemy;
    [SerializeField] private Transform _targetPosition;
    [SerializeField] private Transform _spawnPosition;

    private int _countStekEnemy;
    private int _currentWave;

    private void Awake()
    {
        _currentWave = 1;
        _countStekEnemy = 1;
    }

    private void Update()
    {
        UpdateSpawn();
    }
    public void UpdateSpawn()
    {
        _currentTime += Time.deltaTime;
        if(_currentTime>=_spawnTimer)
        {
            SpawnEnemy(_countStekEnemy);
            _countStekEnemy += 2;
            _currentWave++;
            //событие  новая волна кол-во врагов и их сила
            _currentTime = 0;
        }
    }

    private void SpawnEnemy(int countEnemy)
    {
        Enemy enemy  = Instantiate(_prefabEnemy, _spawnPosition.position, Quaternion.identity).GetComponent<Enemy>();
        //Warrior enemy = //_prefabEnemy.GetComponent<Warrior>();
        enemy.SetTargetPosition(_targetPosition);
        enemy.InitEnemy(1,2,2, countEnemy);
        
    }
}
