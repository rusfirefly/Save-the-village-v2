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
    [SerializeField] private PlayerData _playerData;
    
    public void Initialize()
    {
        CreateListerEvents();
    }

    private void OnDestroy()
    {
        DestroyListerEvents();
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
        _goldText.text = $"{Storage.Gold} +({Population.CountGoldWorker * _playerData.goldMiningPerCycle})";
        _meatText.text = $"{Storage.Meat} +({Population.CountMeatWorker * _playerData.meatMiningPerCycle})";
        _woodText.text = $"{Storage.Wood} +({Population.CountWoodWorker * _playerData.woodMiningPerCycle})";
    }

    private void UpdatePopulationPanel()
    {
        _workerText.text = $"{Population.WorkersCount}/{Population.WorkersCountTotal}";
        _wariorText.text = $"{Population.WarriorsCount}/{Population.WarriorsCountTotal}";
        _archerText.text = $"{Population.ArcherCount}/{Population.ArcherCountTotal}";
        _engineerText.text = $"{WorkManEngineer.CountWork}/{Population.EngineerCountTotal}";
    }

}
