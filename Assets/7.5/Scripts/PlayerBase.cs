using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase: IStorage
{
    private int _gold;
    private int _meat;
    private int _wood;

    public int workersCount { get; private set; }
    public int warriorsCount { get; private set; }

    public int countGoldWorker { get;  set; }
    public int countMeatWorker { get;  set; }
    public int countWoodWorker { get;  set; }

    public PlayerBase(int initGoldCount, int initMeatCount, int initWoodCount)
    {
        _gold = initGoldCount;
        _meat = initMeatCount;
        _wood = initWoodCount;

        workersCount = 0;
        warriorsCount = 0;
    }

    public void AddOneWorker() => workersCount++;
    public void AddOneWarrior() => warriorsCount++;

    public void UpdateGold(int newGoldCount1)
    {
        _gold += newGoldCount1;
    }

    public void UpdateMeat(int newMeatCount)
    {
        _meat += newMeatCount;
    }

    public void UpdateWood(int newWoodCount)
    {
        _wood += newWoodCount;
    }

    public int GetGold()
    {
        return _gold;
    }

    public int GetMeat()
    {
        return _meat;
    }

    public int GetWood()
    {
        return _wood;
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
