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

    private GameObject _trainigMessageText;
    [SerializeField] private GameObject _workerPanel;
    [SerializeField] private GameObject _warriorPanel;
    

    private Button button;

    public int countGoldWorker { get; private set; }
    public int countMeatWorker { get; private set; }
    public int countWoodWorker { get; private set; }

    [Header("Цена найма юнитов")]
    [SerializeField] private int _goldUnitTrainigPrice;
    [SerializeField] private int _meatUnitTrainigPrice;
    [SerializeField] private int _woodUnitTrainigPrice;
    [SerializeField] private int _knightTrainigPrice;

    [Header("Время найма рабочих")]
    [SerializeField] private float _goldWorkTrainingTimer;
    [SerializeField] private float _meatWorkTrainingTimer;
    [SerializeField] private float _woodWorkTrainingTimer;

    [Header("Время найма воинов")]
    [SerializeField] private float _knightTrainingTimer;
    [SerializeField] private float _archerTrainingTimer;
    [Header("Цикл потребляемой еды юнита")]
    [SerializeField] private float _knightEatTimer;
    [SerializeField] private float _archerEatTimer;


    [Header("Количество добываемого ресурса за цикл")]
    [SerializeField] private int _goldMiningPerCycle;
    [SerializeField] private int _meatMiningPerCycle;
    [SerializeField] private int _woodMiningPerCycle;

    [Header("Время сбора ресурсов")]
    [SerializeField] private float _timeGoldMine;
    [SerializeField] private float _timeMeatMine;
    [SerializeField] private float _timeWoodMine;

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
        ShowUnitsPanel(_workerPanel, false);
        ShowUnitsPanel(_warriorPanel, false);
        FindCamps();
        CreateEventsCamp();
        FindMessageText();
    }

    private void Start()
    {
        UpdateStoragePanel();

        SetPositionResources();
        SetMiningCycleTime<GoldMine>(_timeGoldMine);
        SetMiningCycleTime<Miratorg>(_timeMeatMine);
        SetMiningCycleTime<SawMill>(_timeWoodMine);
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

        Castle.CastleAttaking += OnCastleAttaking;
    }
    
    private void OnCastleAttaking()
    {
        Warrior[] warriors = GameObject.FindObjectsOfType<Warrior>();
        foreach (Warrior knight in warriors)
            knight.FindEnemyPosition();
    }

    private void RemoveListenerEvents()
    {
        TrainigPanel.Bounten -= OnBounten;
        TrainigPanel.Traiding -= OnTarianigFinish;
        WorkMan.Working -= OnWorking;
        Mining.Work -= OnFinishMining;

        _workingCamps.Selected -= OnSelectedWorking;
        _warriorCamp.Selected -= OnSelectedWarrior;
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

    private void SetPositionResources()
    {
        _workingCamps.goldPosition = _goldMinePosition;
        _workingCamps.meatPosition = _meatPosition;
        _workingCamps.woodPosition = _woodPosition;
    }

    private void CreateEventsCamp()
    {
        _workingCamps.Selected += OnSelectedWorking;
        _warriorCamp.Selected += OnSelectedWarrior;
    }

    private void FindMessageText()
    {
        _trainigMessageText = GameObject.Find("TrainigMessageText");
    }

    private void CreatePlayerBase()
    {
        _playerBase = new PlayerBase(initGoldCount:10, initMeatCount:10,initWoodCount:0);
    }

    private void UpdateStoragePanel()
    {
        _workerText.text = _playerBase.workersCount.ToString();
        _wariorText.text = _playerBase.warriorsCount.ToString();
        _goldText.text = _playerBase.GetGold().ToString();
        _meatText.text = _playerBase.GetMeat().ToString();
        _woodText.text = _playerBase.GetWood().ToString();
    }

    private void OnFinishMining(string tag)
    {
        int mining;
        switch(tag)
        {
            case "GoldMine":
                mining = GetGoldMiningPerCycle() * countGoldWorker;
                UpdateGold(mining);
                break;
            case "MeatMine":
                mining = GetMeatMiningPerCycle() * countMeatWorker;
                UpdateMeat(mining);
                break;
            case "WoodMine":
                mining = GetWoodMiningPerCycle() * countWoodWorker;
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
            Enums.UnitType.Gold => _goldUnitTrainigPrice,
            Enums.UnitType.Meat => _meatUnitTrainigPrice,
            Enums.UnitType.Wood => _woodUnitTrainigPrice,
            Enums.UnitType.Knight => _knightTrainigPrice,
            _ => 0
        };
    }

    public int GetCountGold() =>_playerBase?.GetGold() ?? 0;
    public int GetCountMeat() => _playerBase?.GetMeat() ?? 0;
    public int GetCountwood() => _playerBase?.GetWood() ?? 0;

    public int GetGoldMiningPerCycle() => _goldMiningPerCycle;
    public int GetMeatMiningPerCycle() => _meatMiningPerCycle;
    public int GetWoodMiningPerCycle() => _woodMiningPerCycle;
    

    public void OnTarianigFinish(Enums.UnitType type)
    {
        if (type == Enums.UnitType.Knight)
        {
            _warriorCamp.Training(type);
            Warrior[] warriors = GameObject.FindObjectsOfType<Warrior>();
            if (warriors.Length == 1)
                warriors[0].firstWarrior = true;
        }
        else
            _workingCamps.Training(type);

        AddUnitToBase(type);
    }

    private void AddUnitToBase(Enums.UnitType type)
    {
        switch (type)
        {
            case Enums.UnitType.Gold:
                _playerBase.AddOneWorker();
                break;
            case Enums.UnitType.Meat:
                _playerBase.AddOneWorker();
                break;
            case Enums.UnitType.Wood:
                _playerBase.AddOneWorker();
                break;
            case Enums.UnitType.Knight:
                _playerBase.AddOneWarrior();
                break;
        };
    }

    private void OnWorking(GameObject workMan, Collider2D collider)
    {
        string tag = collider.gameObject.tag;
        switch (tag)
        {
            case "GoldMine":
                countGoldWorker++;
                StartMining(collider, _goldMiningPerCycle * countGoldWorker);
                Destroy(workMan);
                break;
            case "MeatMine":
                countMeatWorker++;
                StartMining(collider, _meatMiningPerCycle * countMeatWorker);
                Destroy(workMan);
                break;
            case "WoodMine":
                countWoodWorker++;
                StartMining(collider, _woodMiningPerCycle * countWoodWorker);
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

    public int GetGoldTriningPrice() =>_goldUnitTrainigPrice;
    public int GetMeatTriningPrice() => _meatUnitTrainigPrice;
    public int GetWoodTriningPrice() => _woodUnitTrainigPrice;
    public int GetKnightTriningPrice() => _knightTrainigPrice;

    public float GetGoldWorkTrainingTimer() =>_goldWorkTrainingTimer;
    public float GetMeatWorkTrainingTimer() => _meatWorkTrainingTimer;
    public float GetWoodWorkTrainingTimer() => _woodWorkTrainingTimer;
    public float GetKnightTrainingTimer() => _knightTrainingTimer;

    public void UpdateGold(int price) => _playerBase.UpdateGold(price);
    public void UpdateMeat(int price) => _playerBase.UpdateMeat(price);
    public void UpdateWood(int price) => _playerBase.UpdateWood(price);

    public void DefaultStatePanel()
    {
        TrainingMessage(show: true);
        AllCampsDeSelect();
        ShowUnitsPanel(_workerPanel, false);
        ShowUnitsPanel(_warriorPanel, false);
    }

    private void AllCampsDeSelect()
    {
        _warriorCamp.DeSelectedCamp();
        _workingCamps.DeSelectedCamp();
    }

    private void NewGame()
    {

    }

    private void GameOver()
    {

    }

    private void OnSelectedWorking()
    {
        TrainingMessage(show: false);
        DeSelectedCamp(_warriorCamp);
        ShowUnitsPanel(_warriorPanel, false);
        ShowUnitsPanel(_workerPanel, true);
    }

    private void OnSelectedWarrior()
    {
        TrainingMessage(show:false);
        DeSelectedCamp(_workingCamps);
        ShowUnitsPanel(_workerPanel, false);
        ShowUnitsPanel(_warriorPanel, true);
    }

    private void TrainingMessage(bool show)=>_trainigMessageText.SetActive(show);

    private void DeSelectedCamp(WorkingCamp camp)
    {
        if (camp.IsSelectedCamp())
        {
            camp.DeSelectedCamp();
        }
    }

    private void DeSelectedCamp(WarriorCamp camp)
    {
        if (camp.IsSelectedCamp())
        {
            camp.DeSelectedCamp();
        }
    }

    private void ShowUnitsPanel(GameObject panel, bool show)
    {
        RectTransform panelPos=null;
        panelPos = panel.GetComponent<RectTransform>();
        if (show)
        {
            panelPos.localPosition = new Vector3(0,panelPos.localPosition.y);
        }else
        {
            panelPos.localPosition = new Vector3(450, panelPos.localPosition.y);
        }

    }
}
