using UnityEngine;

/// <summary>
/// для настройки...всё PUBLIC жуть
/// </summary>

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    [Header("Цена найма юнитов")]
    public int goldUnitTrainigPrice;
    public int meatUnitTrainigPrice;
    public int woodUnitTrainigPrice;
    public int warriorTrainigPrice;
    public int archerTrainigPrice;

    [Header("Время найма рабочих")]
    public float goldWorkTrainingTimer;
    public float meatWorkTrainingTimer;
    public float woodWorkTrainingTimer;

    [Header("Время найма воинов")]
    public float warriorTrainingTimer;
    public float archerTrainingTimer;

    [Header("Цикл потребляемой еды юнита")]
    public float warriorEatTimer;
    public float archerEatTimer;
    
    [Header("Кол-во потребляеомй еды юнита за цикл")]
    public int warriorEatUpCycle;
    public int archerEatUpCycle;

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

    [Header("Условия победы (выжить в течении N дней)")]
    public int daysToSurvive;

}

