using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase 
{
    public static int gold { get; private set; }
    public static int meat { get; private set; }
    public static int wood { get; set; }

    public int workersCount { get; private set; }
    public int warriorsCount { get; private set; }

    public int countGoldWorker { get;  set; }
    public int countMeatWorker { get;  set; }
    public int countWoodWorker { get;  set; }

    public PlayerBase(int initGoldCount, int initMeatCount, int initWoodCount)
    {
        gold = initGoldCount;
        meat = initMeatCount;
        wood = initWoodCount;

        workersCount = 0;
        warriorsCount = 0;
    }

    public void AddOneWorker() => workersCount++;
    public void AddOneWarrior() => warriorsCount++;
    public void DeathWarrior()=> warriorsCount--;

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
        };
    }

}
