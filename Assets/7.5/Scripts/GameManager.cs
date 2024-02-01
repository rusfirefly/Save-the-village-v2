using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Text _workerText;
    [SerializeField] private Text _wariorText;

    [SerializeField] private Text _goldText;
    [SerializeField] private Text _meatText;
    [SerializeField] private Text _woodText;

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

    
    private void Awake()
    {
        CreatePlayerBase();
        FindCamps();
        CreateEventsCamp();
    }

    private void Start()
    {
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
        TrainigPanel.Bounten += OnBounten;
        TrainigPanel.Traiding += OnTarianigFinish;
        WorkMan.Working += OnWorking;
        Mining.Work += OnFinishMining;
        Warrior.Deathing += OnDeathWarrior;
        Castle.Attacked += OnCastleAttaked;
        Castle.Destroyed += OnCastleDestroyed;
    }
    
    private void OnCastleAttaked()
    {
        Warrior[] warriors = FindObjectsOfType<Warrior>();
        foreach (Warrior knight in warriors)
            knight.FindEnemyPosition();
    }

    private void OnCastleDestroyed()
    {
        GameOver();
    }

    private void RemoveListenerEvents()
    {
        TrainigPanel.Bounten -= OnBounten;
        TrainigPanel.Traiding -= OnTarianigFinish;
        WorkMan.Working -= OnWorking;
        Mining.Work -= OnFinishMining;
        Warrior.Deathing -= OnDeathWarrior;
    }

    private void FindCamps()
    {
        _workingCamps = GameObject.Find("WorkerCamp").GetComponent<WorkingCamp>();
        _warriorCamp = GameObject.Find("WariorCamp").GetComponent<WarriorCamp>();
    }

    private void SetMiningCycleTime<T>(float timeMine) where T : UnityEngine.Object
    {
        T[] mines = FindObjectsOfType<T>();
        foreach (T mine in mines)
            (mine as Mining).SetCycleMining(timeMine);
    }

    private void OnDeathWarrior()=>_playerBase.DeathWarrior();

    private void SetPositionResources()
    {
        _workingCamps.goldPosition = _goldMinePosition;
        _workingCamps.meatPosition = _meatPosition;
        _workingCamps.woodPosition = _woodPosition;
    }

    private void CreateEventsCamp()
    {
        SelectedBuilding.Selected += OnSelectedCamp;
    }

    private void OnSelectedCamp(GameObject gameObject)
    {
        TrainingMessage(show: false);
    }

    private void CreatePlayerBase()
    {
        _playerBase = new PlayerBase(initGoldCount:10, initMeatCount:10,initWoodCount:0);
    }

    private void UpdateStoragePanel()
    {
        _workerText.text = _playerBase.workersCount.ToString();
        _wariorText.text = _playerBase.warriorsCount.ToString();
        _goldText.text = PlayerBase.gold.ToString();
        _meatText.text = PlayerBase.meat.ToString();
        _woodText.text = PlayerBase.wood.ToString();
    }

    private void OnFinishMining(string tag)
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

    private void OnBounten(Enums.UnitType type)
    {
        int price = GetPrice(type);
        UpdateGold(price * -1);
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

    private void OnTarianigFinish(Enums.UnitType type)
    {
        //звук - воин готов к защите
        //звук - работик готов

        switch(type)
        {
            case Enums.UnitType.Knight:
                _warriorCamp.Training(type);
                Warrior[] warriors = GameObject.FindObjectsOfType<Warrior>();
                if (warriors.Length == 1)
                    warriors[0].firstWarrior = true;
                break;
            default:
                _workingCamps.Training(type);

                break;
        }
        _playerBase.AddUnitToBase(type);
    }


    private void OnWorking(GameObject workMan, Collider2D collider)
    {
        //звук - приступаю к работе
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                _playerBase.countGoldWorker++;
                StartMining(collider, _playerData.goldMiningPerCycle * _playerBase.countGoldWorker);
                Destroy(workMan);
                break;
            case "MeatMine":
                _playerBase.countMeatWorker++;
                StartMining(collider, _playerData.meatMiningPerCycle * _playerBase.countMeatWorker);
                Destroy(workMan);
                break;
            case "WoodMine":
                _playerBase.countWoodWorker++;
                StartMining(collider, _playerData.woodMiningPerCycle * _playerBase.countWoodWorker);
                Destroy(workMan);
                break;
        }
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

    private void NewGame()
    {

    }

    private void GameOver()
    {
        GameMenu.menuInstance.ShowGameOverMenu();
    }

    private void TrainingMessage(bool show)=>_trainigMessageText.SetActive(show);

}
