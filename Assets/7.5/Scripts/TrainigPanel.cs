using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainigPanel : MonoBehaviour
{
    public static event Action<Enums.UnitType> Bounten;
    public static event Action<Enums.UnitType> Traiding;
    public bool _isTrainig { get; private set; }
    public bool _isActiveTrainig { get; private set; }

    [SerializeField] private int _trainigPrice;
    [SerializeField] private Text _coldownText;
    [SerializeField] private Image _trainigProgressPanel;
    [SerializeField] private Text _priceText;

    [SerializeField] private Enums.UnitType _unitType;

    
    private float _workTrainingTimer;
    private float _workProgress;

    private GameManager _gameManager;
    private int countResource = 0;
    private bool isBought { get; set; }

    private void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdatePanelInfo(_priceText);
    }

    private void Update()
    {
        UpdatePanelInfo(_priceText);
        AnimationTraining();
    }
    
    public TrainigPanel(Text coldownText, Image trainigProgress)
    {
        _coldownText = coldownText;
        _trainigProgressPanel = trainigProgress;
    }
    
    private void UpdatePanelInfo(Text trainigPriceText)
    {
        switch (_unitType)
        {
            case Enums.UnitType.Gold:
                SetTrainingTime(_gameManager.GetGoldWorkTrainingTimer());
                SetTrainingPrice(_gameManager.GetGoldTriningPrice());
                break;
            case Enums.UnitType.Meat:
                SetTrainingTime(_gameManager.GetMeatWorkTrainingTimer());
                SetTrainingPrice(_gameManager.GetMeatTriningPrice());
                break;
            case Enums.UnitType.Wood:
                SetTrainingTime(_gameManager.GetWoodWorkTrainingTimer());
                SetTrainingPrice(_gameManager.GetWoodTriningPrice());
                break;
            case Enums.UnitType.Knight:
                SetTrainingTime(_gameManager.GetKnightTrainingTimer());
                SetTrainingPrice(_gameManager.GetKnightTriningPrice());
                break;

        }

        countResource = _gameManager.GetCountGold();
        trainigPriceText.text = _trainigPrice.ToString();

        if (countResource >= _trainigPrice)
        {
            _trainigProgressPanel.fillAmount = 0;
            if(!isBought) 
                _isActiveTrainig = true;
        }
        else
        {
            _trainigProgressPanel.fillAmount = 1f;
            if(!isBought)
                _isActiveTrainig = false;
        }
    }

    private void SetTrainingTime(float workTrainingTime) => _workTrainingTimer = workTrainingTime;

    private void SetTrainingPrice(int trainigPrice) => _trainigPrice = trainigPrice;

    public void TrainigWorking()
    {
        if (!_isActiveTrainig) return;
        if (_isTrainig) return;

        if (!isBought)
        {
            OnBounten(_unitType);
            isBought = true;
        }

        _isTrainig = true;
        _workProgress = _workTrainingTimer;
        SetColdownText($"{_workProgress.ToString("#")}s");
    }

    private void OnBounten(Enums.UnitType type) => Bounten?.Invoke(type);
    public  void OnTarianigFinish(Enums.UnitType unitType) => Traiding?.Invoke(unitType);
   
    private void AnimationTraining()
    {
        if (_isTrainig && _isActiveTrainig)
        {
            UpdateProgress(ref _workProgress, _workTrainingTimer, _coldownText, _trainigProgressPanel);
            if (_workProgress <= 0)
            {
                isBought = false;
                UpdateInfo();
                OnTarianigFinish(_unitType);
            }
        }
    }
    private void UpdateProgress(ref float progress, float trainingTime, Text coldownText, Image progressPanel)
    {
        progress -= Time.deltaTime;
        coldownText.text = $"{progress.ToString("#")}s";
        progressPanel.fillAmount = progress / trainingTime;
    }

    private void SetColdownText(string text="")=> _coldownText.text = "";

    private void UpdateInfo()
    {
        _isTrainig = false;
        SetColdownText();
    }


}
