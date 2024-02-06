using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceStorage
{
    public int gold;
    public int meat;
    public int wood;
}

public struct UnitStorage
{
    public int workersCount;
    public int workersCountMaximum;

    public int warriorsCount;
    public int warriorsCountMaximum;

    public int archerCount;
    public int archerCountMaximum;

    public int warriorsCountTotal;
    public int warriorsCountDeath;

    public int countGoldWorker;
    public int countMeatWorker;
    public int countWoodWorker;
}

public class PlayerBase
{
    public static ResourceStorage resourceStorage { get; private set; }
    public UnitStorage unitStorage { get; private set; }

    //------------
    public static int gold { get; private set; }
    public static int meat { get; private set; }
    public static int wood { get; private set; }
    //----------------

    public int workersCount { get; private set; }
    public int warriorsCount { get; private set; }
    public int archerCount { get; private set; }
    public int warriorsCountTotal { get; private set; }
    public int warriorsCountDeath { get; private set; }

    public int countGoldWorker { get; private set; }
    public int countMeatWorker { get; private set; }
    public int countWoodWorker { get; private set; }
    //--------------------

    public PlayerBase(int initGoldCount, int initMeatCount, int initWoodCount)
    {
        gold = initGoldCount;
        meat = initMeatCount;
        wood = initWoodCount;

        workersCount = 0;
        warriorsCount = 0;
        warriorsCountTotal = 0;

        Castle.Repair += OnRepairEvent;
        Warrior.EatUp += OnEatUpEvent;
        Archer.EatUp += OnEatUpEvent;
    }

    public void AddOneWorker() => workersCount++;

    public void AddOneGoldWorker() => countGoldWorker++;

    public void AddOneMeatWorker() => countMeatWorker++;

    public void AddOneWoodWorker() => countWoodWorker++;

    public void AddOneWarrior()
    {
        warriorsCount++;
        warriorsCountTotal++;
    }

    public void DeathWarrior()
    {
        warriorsCount--;
        warriorsCountDeath++;
    }

    public void UpdateGold(int newGoldCount1)
    {
        gold += newGoldCount1;
    }

    public void UpdateMeat(int newMeatCount)
    {
        meat += newMeatCount;
    }

    public void UpdateWood(int newWoodCount)
    {
        wood += newWoodCount;
    }

    public void Reload()
    {
        countGoldWorker = 0;
        countMeatWorker = 0;
        countWoodWorker = 0;

        workersCount = 0;
        warriorsCount = 0;
        warriorsCountTotal = 0;
        warriorsCountDeath = 0;

        Castle.Repair -= OnRepairEvent;
        Warrior.EatUp -= OnEatUpEvent;
    }

    public void AddUnitToBase(Enums.UnitType type)
    {
        switch (type)
        {
            case Enums.UnitType.Gold:
                AddOneWorker();
                break;
            case Enums.UnitType.Meat:
                AddOneWorker();
                break;
            case Enums.UnitType.Wood:
                AddOneWorker();
                break;
            case Enums.UnitType.Knight:
                AddOneWarrior();
                break;
            case Enums.UnitType.Archer:
                AddOneWarrior();
                break;
        };
    }

    private void OnRepairEvent(int price) => wood -= price;
    private void OnEatUpEvent(int eatUp) => meat -= eatUp;
}
