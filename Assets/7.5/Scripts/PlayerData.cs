using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [Header("Tutorial")]
    public bool viewTutorial;
    [Header("���� ����� ������")]
    public int goldUnitTrainigPrice;
    public int meatUnitTrainigPrice;
    public int woodUnitTrainigPrice;
    public int warriorTrainigPrice;
    public int archerTrainigPrice;

    [Header("����� ����� �������")]
    public float goldWorkTrainingTimer;
    public float meatWorkTrainingTimer;
    public float woodWorkTrainingTimer;

    [Header("����� ����� ������")]
    public float warriorTrainingTimer;
    public float archerTrainingTimer;

    [Header("���� ������������ ��� �����")]
    public float warriorEatTimer;
    public int warriorEatUpCycle;
    public float archerEatTimer;
    public int archerEatUpCycle;

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

    public void SetDefaultValue()
    {
        numberWave = 0;
        countEnemyNextWave = 1;
    }

}

