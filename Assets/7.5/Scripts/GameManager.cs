using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public CustomMask customLayerMask;
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

    private float _timeUpdateSortingLayer=1f;
    private int _enemiesDestroyed;
    private void Awake()
    {
        NewGame();
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

        StartCoroutine(UpdateWarriorLayerOrder());
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

    IEnumerator UpdateWarriorLayerOrder()
    {
        SortingLayerWarrior();
        yield return new WaitForSeconds(_timeUpdateSortingLayer);
        StartCoroutine(UpdateWarriorLayerOrder());
    }
//---------------------------------
    EntityInfo[] warriorsPosition;
    Entity[] warriors;

    private void SortingLayerWarrior()
    {
        warriors = FindObjectsOfType<Entity>();
        if(warriors.Length > 0)
            warriorsPosition = new EntityInfo[warriors.Length];
        int index = 0;
        foreach (Entity entity in warriors)
        {
            warriorsPosition[index].entity = entity;
            warriorsPosition[index].positionY = entity.transform.localPosition.y;
            index++;
        }

        if (warriorsPosition!=null)
        {
            QuickSort(warriorsPosition, 0, warriorsPosition.Length - 1);
            for (int i = 0; i < warriorsPosition.Length; i++)
            {
                warriorsPosition[i].entity.SetNewLayer(warriorsPosition.Length - i);
            }
        }
        warriorsPosition = null;
        warriors = null;
    }

    public static void QuickSort(EntityInfo[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);
            if (pivot > 0)
                QuickSort(arr, left, pivot - 1);
            if (pivot < arr.Length - 1)
                QuickSort(arr, pivot + 1, right);
        }
    }

    public static int Partition(EntityInfo[] arr, int left, int right)
    {
        float pivot = arr[left].positionY;
        while (true)
        {
            while (arr[left].positionY < pivot)
                left++;
            while (arr[right].positionY > pivot)
                right--;
            if (left < right)
            {
                if (arr[left].positionY == arr[right].positionY)
                    return right;

                EntityInfo temp = arr[left];
                arr[left] = arr[right];
                arr[right] = temp;
            }
            else
            {
                return right;
            }
        }
    }

    //------------------------

    private void CreateListenerEvents()
    {
        TrainigPanel.Bounten += OnBounten;
        TrainigPanel.Traiding += OnTarianigFinish;
        WorkMan.Working += OnWorking;
        Mining.Work += OnFinishMining;
        Warrior.Deathing += OnDeathWarrior;
        Castle.Attacked += OnCastleAttaked;
        Castle.Destroyed += OnCastleDestroyed;
        Enemy.Deathing += OnEnemiesDestroyed;
    }
    
    private void OnEnemiesDestroyed()
    {
        _enemiesDestroyed++;
    }
    private void OnCastleAttaked()
    {
        Warrior[] warriors = FindObjectsOfType<Warrior>();
        foreach (Warrior knight in warriors)
            knight.FindEnemyPosition();
    }

    private void OnCastleDestroyed()
    {
        GameOver("GAME OVER");
    }

    private void RemoveListenerEvents()
    {
        TrainigPanel.Bounten -= OnBounten;
        TrainigPanel.Traiding -= OnTarianigFinish;
        WorkMan.Working -= OnWorking;
        Mining.Work -= OnFinishMining;
        Warrior.Deathing -= OnDeathWarrior;
        Enemy.Deathing -= OnEnemiesDestroyed;
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

      
      //  GameOver("VIKTORY");
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


    private void OnWorking(GameObject workMan, Collider2D collider)
    {
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
        CreatePlayerBase();
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
