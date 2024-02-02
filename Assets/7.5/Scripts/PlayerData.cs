using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [Header("���� ����� ������")]
    public int goldUnitTrainigPrice;
    public int meatUnitTrainigPrice;
    public int woodUnitTrainigPrice;
    public int knightTrainigPrice;

    [Header("����� ����� �������")]
    public float goldWorkTrainingTimer;
    public float meatWorkTrainingTimer;
    public float woodWorkTrainingTimer;

    [Header("����� ����� ������")]
    public float knightTrainingTimer;
    public float archerTrainingTimer;

    [Header("���� ������������ ��� �����")]
    public float knightEatTimer;
    public float archerEatTimer;

    [Header("���������� ����������� ������� �� ����")]
    public int goldMiningPerCycle;
    public int meatMiningPerCycle;
    public int woodMiningPerCycle;

    [Header("����� ����� ��������")]
    public float timeGoldMine;
    public float timeMeatMine;
    public float timeWoodMine;

    [Header("��������� ����")]
    public int numberWave;
    public float waveCycleTime;
    public int countEnemyNextWave;

    [Header("������� ������")]
    public int countGolds;
    public int countMeats;
    public int countWoods;
    public int countWarriors;
    public int countWorkers;

    public int survivedTheWaves;


}

