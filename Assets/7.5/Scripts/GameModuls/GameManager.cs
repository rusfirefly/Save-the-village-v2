using UnityEngine;
using static Enums;

public class GameManager : MonoBehaviour
{
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

    private int _enemiesDestroyed;

    public void Initialize()
    {
        CreatePlayerBase();
        FindCamps();
        SetPositionResources();

        SetMiningCycleTime<GoldMine>(_playerData.timeGoldMine);
        SetMiningCycleTime<Miratorg>(_playerData.timeMeatMine);
        SetMiningCycleTime<SawMill>(_playerData.timeWoodMine);
    }

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
        TrainigButton.Traiding += OnTarianigFinishEvent;
        Castle.Attacked += OnCastleAttakedEvent;
        Castle.Destroyed += OnCastleDestroyedEvent;
        Enemy.Deathing += OnEnemiesDestroyedEvent;
    }

    private void RemoveListenerEvents()
    {
        SelectedBuilding.Selected -= OnSelectedCampEvent;
        TrainigButton.Traiding -= OnTarianigFinishEvent; 
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
            knight.FindEnemyPosition(_castlePosition.position);
    }

    private void OnCastleDestroyedEvent()
    {
        GameOver(GameOverType.Lose);
    }

    private void FindCamps()
    {
        _workingCamps = FindObjectOfType<WorkingCamp>();
        _warriorCamp = FindObjectOfType<WarriorCamp>();
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
        _workingCamps.GoldPosition = _goldMinePosition;
        _workingCamps.MeatPosition = _meatPosition;
        _workingCamps.WoodPosition = _woodPosition;
    }

    private void CreatePlayerBase()
    {
        _playerBase = new PlayerBase(_playerData, initGoldCount: 12, initMeatCount: 0, initWoodCount: 0);
    }

    private void OnSelectedCampEvent(GameObject gameObject)
    {
        TrainingMessage(show: false);
    }

    private void OnTarianigFinishEvent(Enums.UnitType type)
    {
        switch(type)
        {
            case UnitType.Knight:
                _warriorCamp.Training(type);
                break;
            case UnitType.Archer:
                _warriorCamp.Training(type);
                break;
            default:
                _workingCamps.Training(type);
                break;
        }
        ChekVictoryConditions();
    }

    public void DefaultStatePanel()
    {
       TrainingMessage(show: true);
       SelectedBuilding.AllCampsDeSelect();
    }

    public void ReloadGame()
    {
        DefaultStatePanel();
        ReloadAllMining();
        ReloadAllEntity();
        DeselectAllBuild();
        ReloadMenu();
        ReloadCatle();
        ReloadSpawner();
        ReloadHouses();
        ReloadPlayerBase();
        TrainigReload();
    }

    private void ReloadMenu() => GameMenu.menuInstance.Reload();

    private void ReloadPlayerBase() => _playerBase.ReloadValue();

    private void DeselectAllBuild() => SelectedBuilding.AllCampsDeSelect();

    private void TrainigReload() => TrainigButton.IsReload = true;

    private void ReloadSpawner()
    {
        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.Reload();
    }

    private void ReloadHouses()
    {
        House[] houses = FindObjectsOfType<House>();
        foreach (House hous in houses)
            hous.Reload();
    }

    private void ReloadCatle()
    {
        Castle castle = FindObjectOfType<Castle>();
        castle.Reload();
    }

    private void ReloadAllMining()
    {
        Mining[] miningBuilds = FindObjectsOfType<Mining>();
        foreach (Mining bulding in miningBuilds)
            bulding.Reload();
    }

    private void ReloadAllEntity()
    {
        Entity[] entitys = FindObjectsOfType<Entity>();
        foreach (Entity entity in entitys)
            Destroy(entity.gameObject);
    }

    private void GameOver(GameOverType type)
    {
        
        string statistic = $"Итоги игры:\n" +
                           $"Волн пережито: {_playerData.numberWave}\n" +
                           $"Врагов уничтожено:{_enemiesDestroyed}\n"+
                           $"Нанято воинов: {Population.WarriorHired + Population.ArcherHired}\n" +
                           $"Воинов погибло:{Population.WarriorsCountDeath}\n" +
                           $"Рабочих нанято:{Population.WorkersCount}\n" +
                           $"Собрано золота:{Storage.Gold}\n" +
                           $"Собрано мяса:{Storage.Meat}\n" +
                           $"Собрано дерева:{Storage.Wood}";

        GameMenu.menuInstance.ShowGameOverMenu(statistic, type);
    }

    private void ChekVictoryConditions()
    {
        if (Population.WarriorsCount + Population.ArcherCount == _playerData.numberOfWarriorsHired)
        {
            GameOver(GameOverType.Victory);
        }
    }

    private void TrainingMessage(bool show) =>_trainigMessageText.SetActive(show);

}
