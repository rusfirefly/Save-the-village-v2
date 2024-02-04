using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //в hud class
    [SerializeField] private Text _workerText;
    [SerializeField] private Text _wariorText;

    [SerializeField] private Text _goldText;
    [SerializeField] private Text _meatText;
    [SerializeField] private Text _woodText;
    //------------------------------------------

    private PlayerBase _playerBase;
    private WorkingCamp _workingCamps;
    private WarriorCamp _warriorCamp;

    [SerializeField] private GameObject _trainigMessageText;
    [Header("Настройки для игры")]
    [SerializeField] private PlayerData _playerData;
    [Header("Позиции объектов")]
    [SerializeField] private Transform _castlePosition;
    [SerializeField] private Transform _goldMinePosition;
    [SerializeField] private Transform _meatPosition;
    [SerializeField] private Transform _woodPosition;
    [SerializeField] private Transform _warriorPosition;
    [SerializeField] private Transform _archerPosition;

    private int _enemiesDestroyed;//сделать struct GameStatistic

    public void Initialize()
    {
        NewGame();
        FindCamps();
        UpdateStoragePanel();
        SetPositionResources();

        SetMiningCycleTime<GoldMine>(_playerData.timeGoldMine);
        SetMiningCycleTime<Miratorg>(_playerData.timeMeatMine);
        SetMiningCycleTime<SawMill>(_playerData.timeWoodMine);
    }

    private void FixedUpdate() => UpdateStoragePanel();

    private void OnEnable()
    {
        CreateListenerEvents();
    }

    private void OnDisable()
    {
        RemoveListenerEvents();
    }

    private void CreateListenerEvents()
    {
        SelectedBuilding.Selected += OnSelectedCampEvent;
        TrainigPanel.Bounten += OnBountenEvent;
        TrainigPanel.Traiding += OnTarianigFinishEvent;
        WorkMan.Working += OnWorkingEvent;
        Mining.Work += OnFinishMiningEvent;
        Warrior.Deathing += OnDeathWarriorEvent;
        Castle.Attacked += OnCastleAttakedEvent;
        Castle.Destroyed += OnCastleDestroyedEvent;
        Enemy.Deathing += OnEnemiesDestroyedEvent;
    }
    private void RemoveListenerEvents()
    {
        SelectedBuilding.Selected -= OnSelectedCampEvent;
        TrainigPanel.Bounten -= OnBountenEvent;
        TrainigPanel.Traiding -= OnTarianigFinishEvent;
        WorkMan.Working -= OnWorkingEvent;
        Mining.Work -= OnFinishMiningEvent;
        Warrior.Deathing -= OnDeathWarriorEvent;
        Castle.Attacked -= OnCastleAttakedEvent;
        Castle.Destroyed -= OnCastleDestroyedEvent;
        Enemy.Deathing -= OnEnemiesDestroyedEvent;
    }

    private void OnEnemiesDestroyedEvent()
    {
        _enemiesDestroyed++;
    }

    private void OnCastleAttakedEvent()
    {
        Warrior[] warriors = FindObjectsOfType<Warrior>();
        foreach (Warrior knight in warriors)
            knight.FindEnemyPosition();
    }

    private void OnCastleDestroyedEvent()
    {
        GameOver("GAME OVER");
    }

    private void FindCamps()
    {
        _workingCamps = GameObject.Find("WorkerCamp").GetComponent<WorkingCamp>();
        _warriorCamp = GameObject.Find("WariorCamp").GetComponent<WarriorCamp>();
    }

    private T[] SetMiningCycleTime<T>(float timeMine) where T : UnityEngine.Object
    {
        T[] mines = FindObjectsOfType<T>();
        foreach (T mine in mines)
            (mine as Mining).SetCycleMining(timeMine);

        return mines;
    }
    private void SetPositionResources()
    {
        _workingCamps.goldPosition = _goldMinePosition;
        _workingCamps.meatPosition = _meatPosition;
        _workingCamps.woodPosition = _woodPosition;
    }

    private void OnDeathWarriorEvent() => _playerBase.DeathWarrior();

    private void CreatePlayerBase()
    {
        _playerBase = new PlayerBase(initGoldCount: 10, initMeatCount: 10, initWoodCount: 0);
    }

    private void UpdateStoragePanel()
    {
        _workerText.text = $"{_playerBase.workersCount}";
        _wariorText.text = $"{_playerBase.warriorsCount}";
        _goldText.text = $"{PlayerBase.gold }";
        _meatText.text = $"{PlayerBase.meat }";
        _woodText.text = $"{PlayerBase.wood }";
      //  GameOver("VIKTORY");
    }
    private void OnSelectedCampEvent(GameObject gameObject)
    {
        TrainingMessage(show: false);
    }

    private void OnFinishMiningEvent(string tag)
    {
        int mining;
        switch(tag)
        {
            case "GoldMine":
                mining = _playerData.goldMiningPerCycle *_playerBase.countGoldWorker;
                UpdateGold(mining);
                break;
            case "MeatMine":
                mining = _playerData.meatMiningPerCycle * _playerBase.countMeatWorker;
                UpdateMeat(mining);
                break;
            case "WoodMine":
                mining = _playerData.woodMiningPerCycle * _playerBase.countWoodWorker;
                UpdateWood(mining);
                break;
        }
    }

    private void OnBountenEvent(Enums.UnitType type)
    {
        int price = GetPrice(type);
        UpdateGold(price * -1);
    }

    private void OnTarianigFinishEvent(Enums.UnitType type)
    {
        switch(type)
        {
            case Enums.UnitType.Knight:
                _warriorCamp.Training(type);
                break;
            default:
                _workingCamps.Training(type);

                break;
        }
        _playerBase.AddUnitToBase(type);
    }

    private void OnWorkingEvent(GameObject workMan, Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                _playerBase.AddOneGoldWorker();
                StartMining(collider, _playerData.goldMiningPerCycle * _playerBase.countGoldWorker);
                Destroy(workMan);
                break;
            case "MeatMine":
                _playerBase.AddOneMeatWorker();
                StartMining(collider, _playerData.meatMiningPerCycle * _playerBase.countMeatWorker);
                Destroy(workMan);
                break;
            case "WoodMine":
                _playerBase.AddOneWoodWorker();
                StartMining(collider, _playerData.woodMiningPerCycle * _playerBase.countWoodWorker);
                Destroy(workMan);
                break;
        }
    }
    private int GetPrice(Enums.UnitType type)
    {
        return type switch
        {
            Enums.UnitType.Gold => _playerData.goldUnitTrainigPrice,
            Enums.UnitType.Meat => _playerData.meatUnitTrainigPrice,
            Enums.UnitType.Wood => _playerData.woodUnitTrainigPrice,
            Enums.UnitType.Knight => _playerData.knightTrainigPrice,
            _ => 0
        };
    }
    private void StartMining(Collider2D collider, int countResourcePerCycle)
    {
        Mining mining = collider.gameObject.GetComponent<Mining>();
        mining._inMine = true;
        mining.MinerResourcesPerCycleToText(countResourcePerCycle);
        mining.MineCanvas(true);
    }

    private void UpdateGold(int price) => _playerBase.UpdateGold(price);
    private void UpdateMeat(int price) => _playerBase.UpdateMeat(price);
    private void UpdateWood(int price) => _playerBase.UpdateWood(price);

    public void DefaultStatePanel()
    {
       TrainingMessage(show: true);
       SelectedBuilding.AllCampsDeSelect();
    }

    public void NewGame()
    {
        CreatePlayerBase();
        _playerData.SetDefaultValue();

        Mining[] miningBuilds = FindObjectsOfType<Mining>();
        foreach (Mining bulding in miningBuilds)
            bulding.Reload();

        Entity[] entitys = FindObjectsOfType<Entity>();
        foreach (Entity entity in entitys)
            Destroy(entity.gameObject);

    }

    public void ReloadGame()
    {
        NewGame();
        SelectedBuilding.AllCampsDeSelect();
        GameMenu.menuInstance.Reload();

        Castle castle = FindObjectOfType<Castle>();
        castle.Reload();

        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.Reload();

    }

    private void GameOver(string title)
    {
        string statistic = $"Итоги игры:\n" +
                           $"Волн пережито: {_playerData.numberWave}\n" +
                           $"Врагов уничтоженно:{_enemiesDestroyed}\n"+
                           $"Нането воинов: {_playerBase.warriorsCountTotal}\n" +
                           $"Воинов выжило: {_playerBase.warriorsCount}\n" +
                           $"Воинов погибло:{_playerBase.warriorsCountDeath}\n" +
                           $"Рабочих нанято:{_playerBase.workersCount}\n" +
                           $"Собрано золота:{PlayerBase.gold}\n" +
                           $"Собрано мяса:{PlayerBase.meat}\n" +
                           $"Собрано дерева:{PlayerBase.wood}";

        GameMenu.menuInstance.ShowGameOverMenu(statistic, title);
    }

    private void TrainingMessage(bool show)=>_trainigMessageText.SetActive(show);

}
