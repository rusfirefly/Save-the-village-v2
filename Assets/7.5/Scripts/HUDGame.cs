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
        //UpdateStoragePanel();
        StartCoroutine(UpdatePanels());
    }

    IEnumerator UpdatePanels()
    {
        UpdateStoragePanel();
        yield return new WaitForSeconds(1f);
        StartCoroutine(UpdatePanels());
    }

    //private void FixedUpdate() => UpdateStoragePanel();

    private void UpdateStoragePanel()
    {
        _workerText.text = $"{Population.WorkersCount}/{Population.WorkersCountTotal}";
        _wariorText.text = $"{Population.WarriorsCount}/{Population.WarriorsCountTotal}";
        _archerText.text = $"{Population.ArcherCount}/{Population.ArcherCountTotal}";
        _goldText.text = $"{Storage.Gold} +({Population.CountGoldWorker * _playerData.goldMiningPerCycle})";
        _meatText.text = $"{Storage.Meat} +({Population.CountMeatWorker * _playerData.meatMiningPerCycle})|" +
                         $"-({Population.WarriorsCount * _playerData.warriorEatUpCycle})";
        _woodText.text = $"{Storage.Wood} +({Population.CountWoodWorker * _playerData.woodMiningPerCycle})";
        _engineerText.text = $"{WorkManEngineer.CountWork}/{Population.EngineerCountTotal}";
    }
}
