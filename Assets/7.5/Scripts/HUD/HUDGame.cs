using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDGame : MonoBehaviour
{

    [SerializeField] private Text _workerText;
    [SerializeField] private Text _wariorText;
    [SerializeField] private Text _archerText;
    [SerializeField] private Text _engineerText;
    [SerializeField] private Text _goldText;
    [SerializeField] private Text _meatText;
    [SerializeField] private Text _woodText;
    [SerializeField] private GameSetup _gameSetup;


    [SerializeField] private Text _cycleWaveText;
    [SerializeField] private Text _countEnemysText;
    [SerializeField] private Text _waveText;
    [SerializeField] private Text _numberDayText;
    [SerializeField] private Text _numberDaydeadlineText;

    [SerializeField] private Text _quesText;

    private TimeSpan _timeSpan;

    public void Initialize()
    {
        CreateListerEvents();
        UpdateStoragePanel();
        UpdatePopulationPanel();
        UpdateQustText();

        GameHadler.ReloadAll += OnReloadAll;
    }

    private void OnDestroy()
    {
        DestroyListerEvents();

        GameHadler.ReloadAll -= OnReloadAll;
    }

    private void OnReloadAll()
    {
        Reload();
    }

    public void SetCycleWaveText(float time)
    {
        _timeSpan = new TimeSpan(0, 0, Mathf.RoundToInt(time));
        _cycleWaveText.text = $"Волна начнётся через: {_timeSpan.Minutes} м {_timeSpan.Seconds} с";
    }

    public void UpdateEnemyInformation(int countEnemy)
    {
        _countEnemysText.text = $"Врагов в следующем набеге: {countEnemy}";
    }

    public void UpdateWaveInfo(int numberWave)
    {
        _waveText.text = $"Волна: {numberWave}";
    }

    public void UpdateNumberDay(int newNumberDay)
    {
        _numberDayText.text = $"День: {newNumberDay}";
    }

    public void UpdateDayToDeadline(int daysLeft)
    {
        _numberDaydeadlineText.text = $"До набега врагов: {daysLeft} д";
    }

    public void HideTextDayToDeadline() => _numberDaydeadlineText.enabled = false;

    public void Reload()
    {
        UpdateStoragePanel();
        UpdatePopulationPanel();
        ShowTextDayToDeadline();
    }

    private void ShowTextDayToDeadline()=> _numberDaydeadlineText.enabled = true;

    private void UpdateQustText()
    {
        _quesText.text = $"Задание:\n-выжить в течении {_gameSetup.daysToSurvive} д";
    }

    private void CreateListerEvents()
    {
        PlayerBase.StorageUpdate += OnStorageUpdate;
        PlayerBase.PopulationUpdate += OnPopulationUpdate;
        WorkManEngineer.PopulationUpdate += OnPopulationUpdate;
    }

    private void DestroyListerEvents()
    {
        PlayerBase.StorageUpdate -= OnStorageUpdate;
        PlayerBase.PopulationUpdate -= OnPopulationUpdate;
        WorkManEngineer.PopulationUpdate -= OnPopulationUpdate;
    }

    private void OnStorageUpdate()
    {
        UpdateStoragePanel();
    }

    private void OnPopulationUpdate()
    {
        UpdatePopulationPanel();

    }

    private void UpdateStoragePanel()
    {
        _goldText.text = $"{Storage.Gold} +({GetMiningGoldPerCycly()})";
        _meatText.text = $"{Storage.Meat} +({GetMiningMeatPerCycly()}) [-{GetMeatEatAllWarior()}] eatUp";
        _woodText.text = $"{Storage.Wood} +({GetMiningWoodPerCycly()})";
    }

    private void UpdatePopulationPanel()
    {
        _workerText.text = $"{Population.WorkersCount}/{Population.WorkersCountTotal}";
        _wariorText.text = $"{Population.WarriorsCount}/{Population.WarriorsCountTotal}";
        _archerText.text = $"{Population.ArcherCount}/{Population.ArcherCountTotal}";
        _engineerText.text = $"{WorkManEngineer.CountWork}/{Population.EngineerCountTotal}";
    }

    private int GetMiningGoldPerCycly()
    {
        return Population.CountGoldWorker * _gameSetup.goldMiningPerCycle;
    }

    private int GetMiningMeatPerCycly()
    {
        return Population.CountMeatWorker * _gameSetup.meatMiningPerCycle;
    }

    private int GetMiningWoodPerCycly()
    {
        return Population.CountWoodWorker * _gameSetup.woodMiningPerCycle;
    }

    private int GetMeatEatAllWarior()
    {
        return (Population.WarriorsCount * _gameSetup.warriorEatUpCycle) +
                (Population.ArcherCount * _gameSetup.archerEatUpCycle);
    }


}
