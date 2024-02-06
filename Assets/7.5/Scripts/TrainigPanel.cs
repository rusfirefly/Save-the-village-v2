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
    [SerializeField] private SoundClip _sound;

    private float _workTrainingTimer;
    private float _workProgress;

    [SerializeField] private PlayerData _playerData;
    private int _countResource = 0;
    private bool _isBought;
    public static bool isReload;

    private void Start()
    {
        UpdatePanelInfo(_priceText);
        _sound = gameObject.GetComponent<SoundClip>();
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
                SetTrainingTime(_playerData.goldWorkTrainingTimer);
                SetTrainingPrice(_playerData.goldUnitTrainigPrice);
                break;
            case Enums.UnitType.Meat:
                SetTrainingTime(_playerData.meatWorkTrainingTimer);
                SetTrainingPrice(_playerData.meatUnitTrainigPrice);
                break;
            case Enums.UnitType.Wood:
                SetTrainingTime(_playerData.woodWorkTrainingTimer);
                SetTrainingPrice(_playerData.woodUnitTrainigPrice);
                break;
            case Enums.UnitType.Knight:
                SetTrainingTime(_playerData.warriorTrainingTimer);
                SetTrainingPrice(_playerData.warriorTrainigPrice);
                break;
            case Enums.UnitType.Archer:
                SetTrainingTime(_playerData.archerTrainingTimer);
                SetTrainingPrice(_playerData.archerTrainigPrice);
                break;

        }

        _countResource = PlayerBase.gold;
        trainigPriceText.text = _trainigPrice.ToString();

        if (_countResource >= _trainigPrice)
        {
            _trainigProgressPanel.fillAmount = 0;
            if(!_isBought) 
                _isActiveTrainig = true;
        }
        else
        {
            _trainigProgressPanel.fillAmount = 1f;
            if(!_isBought)
                _isActiveTrainig = false;
        }
    }

    private void SetTrainingTime(float workTrainingTime) => _workTrainingTimer = workTrainingTime;

    private void SetTrainingPrice(int trainigPrice) => _trainigPrice = trainigPrice;

    public void TrainigWorking()
    {
        if (!_isActiveTrainig)
        {
            _sound.PlaySound();
            return;
        }

        if (_isTrainig) return;

        if (!_isBought)
        {
            OnBounten(_unitType);
            _isBought = true;
        }

        _isTrainig = true;
        _workProgress = _workTrainingTimer;
        SetColdownText($"{_workProgress:F0}s");
    }

    private void OnBounten(Enums.UnitType type) => Bounten?.Invoke(type);
    public  void OnTarianigFinish(Enums.UnitType unitType) => Traiding?.Invoke(unitType);
   
    private void AnimationTraining()
    {

        if (_isTrainig && _isActiveTrainig)
        {
            if (isReload)
            {
                _isTrainig = false;
                isReload = false;
                _isBought = false;
                UpdateInfo();
                return;
            }

            UpdateProgress(ref _workProgress, _workTrainingTimer, _trainigProgressPanel);
            if (_workProgress <= 0)
            {
                _isBought = false;
                UpdateInfo();
                OnTarianigFinish(_unitType);
            }
        }
    }
    private void UpdateProgress(ref float progress, float trainingTime, Image progressPanel)
    {
        progress -= Time.deltaTime;
        SetColdownText($"{progress:F0}s");
        progressPanel.fillAmount = progress / trainingTime;
    }

    private void SetColdownText(string text = "") => _coldownText.text = text;

    private void UpdateInfo()
    {
        _isTrainig = false;
        SetColdownText();
    }


}
