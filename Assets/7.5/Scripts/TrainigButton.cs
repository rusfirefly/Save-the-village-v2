using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainigButton : MonoBehaviour
{
    public static event Action<Enums.UnitType> Bounten;
    public static event Action<Enums.UnitType> Traiding;
    public static bool IsReload { get; set; }

    private bool _isTrainig;
    private bool _isActiveTrainig;

    [SerializeField] private int _trainigPrice;
    [SerializeField] private Text _coldownText;
    [SerializeField] private Image _trainigProgressPanel;
    [SerializeField] private Text _priceText;

    [SerializeField] private Enums.UnitType _unitType;
    [SerializeField] private PlayerData _playerData;
    private SoundClip _sound;

    private float _workTrainingTimer;
    private float _workProgress;

    private int _countResource = 0;
    private bool _isBought;

    private const byte _activeButton = 0;
    private const byte _deActiveButton = 1;


    private void Start()
    {
        UpdatePanelInfo(_priceText);
        GetSoundClip();
    }

    private void Update()
    {
        UpdatePanelInfo(_priceText);
        AnimationTraining();
    }
    
    public TrainigButton(Text coldownText, Image trainigProgress)
    {
        _coldownText = coldownText;
        _trainigProgressPanel = trainigProgress;
    }
    
    private void GetSoundClip()=> _sound = gameObject.GetComponent<SoundClip>();

    private void UpdatePanelInfo(Text trainigPriceText)
    {
        float trainingTimer = 0;
        int trainigPrice = 0;
        GetTrainingTimeAndTrainigPrice(ref trainingTimer, ref trainigPrice);
        SetTrainingTime(trainingTimer);
        SetTrainingPrice(trainigPrice);
        SetTextPrice(trainigPriceText);
        ActiveTrainig();
    }

    private void GetTrainingTimeAndTrainigPrice(ref float trainingTimer, ref int trainigPrice)
    {
        switch (_unitType)
        {
            case Enums.UnitType.Gold:
                trainingTimer = _playerData.goldWorkTrainingTimer;
                trainigPrice = _playerData.goldUnitTrainigPrice;
                break;
            case Enums.UnitType.Meat:
                trainingTimer = _playerData.meatWorkTrainingTimer;
                trainigPrice = _playerData.meatUnitTrainigPrice;
                break;
            case Enums.UnitType.Wood:
                trainingTimer = _playerData.woodWorkTrainingTimer;
                trainigPrice = _playerData.woodUnitTrainigPrice;
                break;
            case Enums.UnitType.Knight:
                trainingTimer = _playerData.warriorTrainingTimer;
                trainigPrice = _playerData.warriorTrainigPrice;
                break;
            case Enums.UnitType.Archer:
                trainingTimer = _playerData.archerTrainingTimer;
                trainigPrice = _playerData.archerTrainigPrice;
                break;
        }

    }

    private void SetTextPrice(Text trainigPriceText) => trainigPriceText.text = _trainigPrice.ToString();

    private void ActiveTrainig()
    {
        GetCurrentResource();

        if (_countResource >= _trainigPrice)
        {
            TrainigButtonActive(active:true);
        }
        else
        {
            TrainigButtonActive(active:false);
        }
    }

    private void GetCurrentResource()=> _countResource = Storage.Gold;
    private void TrainigButtonActive(bool active)
    {
        _trainigProgressPanel.fillAmount = active?_activeButton:_deActiveButton;
        if (!_isBought)
            _isActiveTrainig = active;
    }

    private void SetTrainingTime(float workTrainingTime) => _workTrainingTimer = workTrainingTime;

    private void SetTrainingPrice(int trainigPrice) => _trainigPrice = trainigPrice;

    public void TrainigWorking()
    {
        if (!_isActiveTrainig)
        {
            PlaySoundNeedGold();
            return;
        }

        if (_isTrainig) return;

        if (!_isBought)
        {
            OnBountenInvoke(_unitType);
            _isBought = true;
        }

        _isTrainig = true;
        _workProgress = _workTrainingTimer;

        SetColdownText($"{_workProgress:F0}s");
    }

    private void PlaySoundNeedGold() => _sound.PlaySound();

    private void OnBountenInvoke(Enums.UnitType type) => Bounten?.Invoke(type);
    public  void OnTarianigFinish(Enums.UnitType unitType) => Traiding?.Invoke(unitType);
   
    private void AnimationTraining()
    {
        if (_isTrainig && _isActiveTrainig)
        {
            if (IsReload)
            {
                ReloadProgressTraining();
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

    private void ReloadProgressTraining()
    {
        _isTrainig = false;
        IsReload = false;
        _isBought = false;
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
