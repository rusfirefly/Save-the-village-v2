using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [Header("Цена найма юнитов")]
    public int goldUnitTrainigPrice;
    public int meatUnitTrainigPrice;
    public int woodUnitTrainigPrice;
    public int knightTrainigPrice;

    [Header("Время найма рабочих")]
    public float goldWorkTrainingTimer;
    public float meatWorkTrainingTimer;
    public float woodWorkTrainingTimer;

    [Header("Время найма воинов")]
    public float knightTrainingTimer;
    public float archerTrainingTimer;

    [Header("Цикл потребляемой еды юнита")]
    public float knightEatTimer;
    public float archerEatTimer;

    [Header("Количество добываемого ресурса за цикл")]
    public int goldMiningPerCycle;
    public int meatMiningPerCycle;
    public int woodMiningPerCycle;

    [Header("Время сбора ресурсов")]
    public float timeGoldMine;
    public float timeMeatMine;
    public float timeWoodMine;

    [Header("настройка волн")]
    public int numberWave;
    public float waveCycleTime;
    public int countEnemyNextWave;

    [Header("Условия победы")]
    public int countGolds;
    public int countMeats;
    public int countWoods;
    public int countWarriors;
    public int countWorkers;

    public int survivedTheWaves;


}

