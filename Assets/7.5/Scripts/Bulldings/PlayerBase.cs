using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage
{
    public static int Gold { get; private set; }
    public static int Meat { get; private set; }
    public static int Wood { get; private set; }

    public Storage(int gold, int meat, int wood)
    {
        Gold = gold;
        Meat = meat;
        Wood = wood;
    }
    
    public void AddGold(int newGoldCount1)=>Gold += newGoldCount1;

    public void AddMeat(int newMeatCount)=>Meat += newMeatCount;

    public void AddWood(int newWoodCount)=>Wood += newWoodCount;

    public void SpendGold(int price) => Gold -= price;

    public void UseWood(int price) => Wood -= price;

    public void UseMeat(int eatUp) => Meat -= eatUp;
}

public class Population
{
    public static int WorkersCount { get; private set; }
    public static int WorkersCountTotal { get; private set; }
    public static int WarriorsCount { get; private set; }
    public static int WarriorsCountTotal { get; private set; }
    public static int ArcherCount { get; private set; }
    public static int ArcherCountTotal { get; private set; }
    public static int WarriorsCountDeath { get; private set; }

    public static int CountGoldWorker { get; private set; }
    public static int CountMeatWorker { get; private set; }
    public static int CountWoodWorker { get; private set; }

    public void AddOneWorker() => WorkersCount++;

    public void AddOneGoldWorker() => CountGoldWorker++;

    public void AddOneMeatWorker() => CountMeatWorker++;

    public void AddOneWoodWorker() => CountWoodWorker++;

    public void AddOneWarrior()
    {
        WarriorsCount++;
    }

    public void DeathWarrior()
    {
        WarriorsCount--;
        WarriorsCountDeath++;
    }

    public void ReloadValue()
    {
        WorkersCount = 0;
        WarriorsCount = 0;
        CountGoldWorker = 0;
        CountMeatWorker = 0;
        CountWoodWorker = 0;

        WorkersCountTotal = 10;
        WarriorsCountTotal = 10;
        ArcherCountTotal = 1;
    }
}

public class PlayerBase
{
    private Storage _storage;
    private Population _population;
    private PlayerData _playerData;

    public PlayerBase(PlayerData playerData, int initGoldCount, int initMeatCount, int initWoodCount)
    {
        _storage = new Storage(initGoldCount, initMeatCount, initWoodCount);
        _population = new();
        _playerData = playerData;

        ReloadValue();

        Castle.Repair += OnRepairEvent;
        Warrior.EatUp += OnEatUpEvent;
        Archer.EatUp += OnEatUpEvent;
        Mining.Work += OnFinishMiningEvent;
        TrainigButton.Bounten += OnBountenEvent;
        Warrior.Deathing += OnDeathWarriorEvent;
        Archer.Deathing += OnDeathWarriorEvent;
        WorkMan.Working += OnWorkingEvent;
    }

    public void ReloadValue() => _population.ReloadValue();

    private void OnFinishMiningEvent(string tag)
    {
        int mining;
        switch (tag)
        {
            case "GoldMine":
                mining = _playerData.goldMiningPerCycle * Population.CountGoldWorker;
                _storage.AddGold(mining);
                break;
            case "MeatMine":
                mining = _playerData.meatMiningPerCycle * Population.CountMeatWorker;
                _storage.AddMeat(mining);
                break;
            case "WoodMine":
                mining = _playerData.woodMiningPerCycle * Population.CountWoodWorker;
                _storage.AddWood(mining);
                break;
        }
    }

    private void OnWorkingEvent(GameObject workMan, Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                _population.AddOneGoldWorker();
                break;
            case "MeatMine":
                _population.AddOneMeatWorker();
                break;
            case "WoodMine":
                _population.AddOneWoodWorker();
                break;
        }
    }

    private void OnBountenEvent(Enums.UnitType type)
    {
        int price = GetPrice(type);
        _storage.SpendGold(price);
    }

    private void OnDeathWarriorEvent() => _population.DeathWarrior();

    private int GetPrice(Enums.UnitType type)
    {
        return type switch
        {
            Enums.UnitType.Gold => _playerData.goldUnitTrainigPrice,
            Enums.UnitType.Meat => _playerData.meatUnitTrainigPrice,
            Enums.UnitType.Wood => _playerData.woodUnitTrainigPrice,
            Enums.UnitType.Knight => _playerData.warriorTrainigPrice,
            Enums.UnitType.Archer => _playerData.archerTrainigPrice,
            _ => 0
        };
    }

    public void AddUnitToBase(Enums.UnitType type)
    {
        switch (type)
        {
            case Enums.UnitType.Gold:
                _population.AddOneWorker();
                break;
            case Enums.UnitType.Meat:
                _population.AddOneWorker();
                break;
            case Enums.UnitType.Wood:
                _population.AddOneWorker();
                break;
            case Enums.UnitType.Knight:
                _population.AddOneWarrior();
                break;
            case Enums.UnitType.Archer:
                _population.AddOneWarrior();
                break;
        };
    }
    private void OnRepairEvent(int price) => _storage.UseWood(price);
    private void OnEatUpEvent(int eatUp) => _storage.UseMeat(eatUp);
}
