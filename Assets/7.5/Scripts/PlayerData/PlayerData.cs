using UnityEngine;

/// <summary>
/// ��� ���������...�� PUBLIC ����
/// </summary>

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
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
    public float archerEatTimer;
    
    [Header("���-�� ������������ ��� ����� �� ����")]
    public int warriorEatUpCycle;
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

    [Header("������� ������ (������ � ������� N ����)")]
    public int daysToSurvive;

}

