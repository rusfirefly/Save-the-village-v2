using System;
using UnityEngine;
using static Enums;

public class GameHadler : MonoBehaviour
{
    public static event Action ReloadAll;

    [SerializeField] private GameObject _trainigMessageText;
    [Header("��������� ��� ����")]
    [SerializeField] private GameSetup _gameSetup;

    private int _enemiesDestroyed;
    private int _day;

    public void Initialize()
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
        if (currentDay == _gameSetup.daysToSurvive)
            GameOver(GameOverType.Victory);
    }

    private void TrainingMessage(bool show) => _trainigMessageText.SetActive(show);

    private void GameOver(GameOverType type)
    {

        string statistic = $"����� ����:\n" +
                           $"���� ������: {_day}\n" +
                           $"���� ��������: {_gameSetup.numberWave}\n" +
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
        ReloadMenu();
        TrainigReload();
        ReloadAll?.Invoke();
    }

    public void DefaultStatePanel()
    {
        TrainingMessage(show: true);
        DeselectAllBuild();
    }

    private void ReloadMenu() => GameMenu.menuInstance.Reload();

    private void DeselectAllBuild() => SelectedBuilding.AllCampsDeSelect();

    private void TrainigReload() => TrainigButton.IsReload = true;

}
