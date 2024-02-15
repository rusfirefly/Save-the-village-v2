using System;
using UnityEngine;
public class PlayerBase
{
    public static event Action StorageUpdate;
    public static event Action PopulationUpdate;

    private Storage _storage;
    private Population _population;
    private PlayerData _playerData;

    public PlayerBase(PlayerData playerData, int initGoldCount, int initMeatCount, int initWoodCount)
    {
        _storage = new Storage(initGoldCount, initMeatCount, initWoodCount);
        _population = new Population();
        _playerData = playerData;

        ReloadValue();
        CreateListenerEvents();
    }

    public void ReloadValue()
    {
        _population.ReloadValue();
        _storage.Reload();
        StorageUpdate?.Invoke();
        PopulationUpdate?.Invoke();
    }

    private void CreateListenerEvents()
    {
        Castle.Repair += OnRepairEvent;
        Warrior.EatUp += OnEatUpEvent;
        Archer.EatUp += OnEatUpEvent;
        Mining.Work += OnFinishMiningEvent;
        TrainigButton.Bounten += OnBountenEvent;
        Warrior.Deathing += OnDeathWarriorEvent;
        Archer.Deathing += OnDeathArcherEvent;
        House.BuildComplete += BuildCompleteEvent;
        WorkMan.Working += OnWorkingEvent;
        House.HouseBought += OnHouseBought;
    }

    public void RemoveListenerEvents()
    {
        Castle.Repair -= OnRepairEvent;
        Warrior.EatUp -= OnEatUpEvent;
        Archer.EatUp -= OnEatUpEvent;
        Mining.Work -= OnFinishMiningEvent;
        TrainigButton.Bounten -= OnBountenEvent;
        Warrior.Deathing -= OnDeathWarriorEvent;
        Archer.Deathing -= OnDeathArcherEvent;
        House.BuildComplete -= BuildCompleteEvent;
        WorkMan.Working -= OnWorkingEvent;
        House.HouseBought -= OnHouseBought;
    }

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

        StorageUpdate?.Invoke();

    }

    private void BuildCompleteEvent()
    {
        _population.UpPopulation();
        PopulationUpdate?.Invoke();
    }

    private void OnWorkingEvent(Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                _population.AddOneGoldWorker();
                StartMining(collider, _playerData.goldMiningPerCycle * Population.CountGoldWorker);
                break;
            case "MeatMine":
                _population.AddOneMeatWorker();
                StartMining(collider, _playerData.meatMiningPerCycle * Population.CountMeatWorker);
                break;
            case "WoodMine":
                _population.AddOneWoodWorker();
                StartMining(collider, _playerData.woodMiningPerCycle * Population.CountWoodWorker);
                break;
        }

        PopulationUpdate?.Invoke();
    }

    private void StartMining(Collider2D collider, int countResourcePerCycle)
    {
        Mining mining = collider.gameObject.GetComponent<Mining>();
        if (!mining) return;
        mining.InMine = true;
        mining.MinerResourcesPerCycleToText(countResourcePerCycle);
        mining.MineCanvas(true);
        StorageUpdate?.Invoke();
    }

    private void OnBountenEvent(Enums.UnitType type)
    {
        AddUnitToBase(type);
        int price = GetPrice(type);
        _storage.SpendGold(price);
        StorageUpdate?.Invoke();
    }

    private void OnDeathWarriorEvent() => _population.DeathWarrior();

    private void OnDeathArcherEvent() => _population.DeathArcher();

    private void OnHouseBought(int price)
    {
        _storage.UseWood(price);
        StorageUpdate?.Invoke();
    }
    

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
                _population.AddOneArcher();
                break;
        };

        PopulationUpdate?.Invoke();
    }

    private void OnRepairEvent(int price) => _storage.UseWood(price);

    private void OnEatUpEvent(int eatUp)
    {
        _storage.UseMeat(eatUp);
        StorageUpdate?.Invoke();
    }
}
