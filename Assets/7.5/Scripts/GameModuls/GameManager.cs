using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerBase _playerBase;
    private WorkingCamp _workingCamps;
    private WarriorCamp _warriorCamp;

    [SerializeField] private GameObject _trainigMessageText;
    [Header("��������� ��� ����")]
    [SerializeField] private PlayerData _playerData;
    [Header("������� ��������")]
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
        _playerBase.RemoveListenerEvents();
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
        GameOver("GAME OVER");
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
            case Enums.UnitType.Knight:
                _warriorCamp.Training(type);
                break;
            case Enums.UnitType.Archer:
                _warriorCamp.Training(type);
                break;
            default:
                _workingCamps.Training(type);
                break;
        }
    }

    public void DefaultStatePanel()
    {
       TrainingMessage(show: true);
       SelectedBuilding.AllCampsDeSelect();
    }

    public void ReloadGame()
    {
        DefaultStatePanel();

        Mining[] miningBuilds = FindObjectsOfType<Mining>();
        foreach (Mining bulding in miningBuilds)
            bulding.Reload();

        Entity[] entitys = FindObjectsOfType<Entity>();
        foreach (Entity entity in entitys)
            Destroy(entity.gameObject);

        SelectedBuilding.AllCampsDeSelect();
        GameMenu.menuInstance.Reload();

        Castle castle = FindObjectOfType<Castle>();
        castle.Reload();

        Spawner spawner = FindObjectOfType<Spawner>();
        spawner.Reload();

        House[] houses = FindObjectsOfType<House>();
        foreach (House hous in houses)
            hous.Reload();

        _playerBase.ReloadValue();
        TrainigButton.IsReload = true;
    }

    private void GameOver(string title)
    {
        
        string statistic = $"����� ����:\n" +
                           $"���� ��������: {_playerData.numberWave}\n" +
                           $"������ ����������:{_enemiesDestroyed}\n"+
                           $"������ ������: {Population.WarriorHired + Population.ArcherHired}\n" +
                           $"������ �������:{Population.WarriorsCountDeath}\n" +
                           $"������� ������:{Population.WorkersCount}\n" +
                           $"������� ������:{Storage.Gold}\n" +
                           $"������� ����:{Storage.Meat}\n" +
                           $"������� ������:{Storage.Wood}";

        GameMenu.menuInstance.ShowGameOverMenu(statistic, title);
    }

    private void TrainingMessage(bool show) =>_trainigMessageText.SetActive(show);

}
