using UnityEngine;
using static Enums;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _trainigMessageText;
    [Header("��������� ��� ����")]
    [SerializeField] private PlayerData _playerData;


    private Castle _castle;
    private Spawner _spawner;

    private int _enemiesDestroyed;
    private int _day;

    public void Initialize()
    {
        _castle = FindObjectOfType<Castle>();
        _spawner = FindObjectOfType<Spawner>();
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
        Castle.Destroyed += OnCastleDestroyedEvent;
        Enemy.Deathing += OnEnemiesDestroyedEvent;
        DayAndNight.NewDay += OnNewDay;
    }

    private void RemoveListenerEvents()
    {
        SelectedBuilding.Selected -= OnSelectedCampEvent;
        Castle.Destroyed -= OnCastleDestroyedEvent;
        Enemy.Deathing -= OnEnemiesDestroyedEvent;
        DayAndNight.NewDay -= OnNewDay;
    }

    private void OnSelectedCampEvent(GameObject gameObject)
    {
        TrainingMessage(show: false);
    }

    private void OnCastleDestroyedEvent()
    {
        GameOver(GameOverType.Lose);
    }

    private void OnEnemiesDestroyedEvent()
    {
        _enemiesDestroyed++;
    }

    private void OnNewDay(int currentDay)
    {
        _day = currentDay;
        if (currentDay == _playerData.daysToSurvive)
            GameOver(GameOverType.Victory);
    }

    private void TrainingMessage(bool show) => _trainigMessageText.SetActive(show);

    private void GameOver(GameOverType type)
    {

        string statistic = $"����� ����:\n" +
                           $"���� ������: {_day}" +
                           $"���� ��������: {_playerData.numberWave}\n" +
                           $"������ ����������:{_enemiesDestroyed}\n" +
                           $"������ ������: {Population.WarriorHired + Population.ArcherHired}\n" +
                           $"������ �������:{Population.WarriorsCountDeath}\n" +
                           $"������� ������:{Population.WorkersCount}\n" +
                           $"������� ������:{Storage.Gold}\n" +
                           $"������� ����:{Storage.Meat}\n" +
                           $"������� ������:{Storage.Wood}";

        GameMenu.menuInstance.ShowGameOverMenu(statistic, type);
    }

    public void ReloadGame()
    {
        DefaultStatePanel();
        ReloadAllMining();
        ReloadAllEntity();
        DeselectAllBuild();
        ReloadMenu();
        ReloadCastle();
        ReloadSpawner();
        ReloadHouses();
        TrainigReload();
    }

    public void DefaultStatePanel()
    {
        TrainingMessage(show: true);
        SelectedBuilding.AllCampsDeSelect();
    }

    private void ReloadMenu() => GameMenu.menuInstance.Reload();


    private void DeselectAllBuild() => SelectedBuilding.AllCampsDeSelect();

    private void TrainigReload() => TrainigButton.IsReload = true;

    private void ReloadSpawner()
    {
        _spawner.Reload();
    }

    private void ReloadHouses()
    {
        House[] houses = FindObjectsOfType<House>();
        foreach (House hous in houses)
            hous.Reload();
    }

    private void ReloadCastle()
    {
        _castle.Reload();
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





}
