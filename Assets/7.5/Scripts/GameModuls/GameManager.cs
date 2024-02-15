using System;
using UnityEngine;
using static Enums;

public class GameManager : MonoBehaviour
{
    public static event Action ReloadAll;

    [SerializeField] private GameObject _trainigMessageText;
    [Header("Настройки для игры")]
    [SerializeField] private PlayerData _playerData;

    private Castle _castle;
    private Spawner _spawner;

    private int _enemiesDestroyed;
    private int _day;
    private DayAndNight _dayNight;

    public void Initialize()
    {
        _castle = FindObjectOfType<Castle>();
        _spawner = FindObjectOfType<Spawner>();
        _dayNight = FindObjectOfType<DayAndNight>();
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

        string statistic = $"Итоги игры:\n" +
                           $"Дней прошло: {_day}" +
                           $"Волн пережито: {_playerData.numberWave}\n" +
                           $"Врагов уничтожено:{_enemiesDestroyed}\n" +
                           $"Нанято воинов: {Population.WarriorHired + Population.ArcherHired}\n" +
                           $"Воинов погибло:{Population.WarriorsCountDeath}\n" +
                           $"Рабочих нанято:{Population.WorkersCount}\n" +
                           $"Собрано золота:{Storage.Gold}\n" +
                           $"Собрано мяса:{Storage.Meat}\n" +
                           $"Собрано дерева:{Storage.Wood}";

        GameMenu.menuInstance.ShowGameOverMenu(statistic, type);
    }

    public void ReloadGame()
    {
        DefaultStatePanel();
       // DeselectAllBuild();

        ReloadAll?.Invoke();


        ReloadAllMining();
        ReloadAllEntity();

        ReloadMenu();
        ReloadCastle();
        ReloadSpawner();
        ReloadHouses();
        TrainigReload();

        _dayNight.Reload();

        WorkMan[] allWorkingMan = FindObjectsOfType<WorkMan>();
        foreach (WorkMan workman in allWorkingMan)
            Destroy(workman.gameObject);

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
