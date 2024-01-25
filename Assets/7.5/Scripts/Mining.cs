using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mining : MonoBehaviour
{
    public static event Action<string> Work;
    public bool _inMine { get; set; }
    private float _mineTimer;
    [SerializeField] private float _mineTimerCycle;

    [SerializeField] private Text _coldownTimeText;
    [SerializeField] private Image _progress;
    [SerializeField] private Canvas _inMineCanvas;
    [SerializeField] private Text _mineCountText;

    private bool _isAnimation;
    private float _positionText;
    private Vector3 _defaulPosition;

    private GameManager _gameManager;
    [SerializeField] private int _countResources;

    private float _timeAnimation = 2f;
    private float _startTimeAnimation = 0;

    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _inMine = false;
        _mineCountText.gameObject.SetActive(false);
        MineCanvas(false);
        _defaulPosition = _mineCountText.rectTransform.position;
        _positionText = _mineCountText.rectTransform.position.y;
    }

    void Update()
    {
        Mine();
        AnimationMineText();
    }

    private void Mine()
    {
        if (_inMine)
        {
            _mineTimer += Time.deltaTime;

            if (_mineTimer >= _mineTimerCycle)
            {
                FinishMining(gameObject.tag);
                _isAnimation = true;
                _mineTimer = 0;
            }
            ProgressFillAmount(_mineTimer);
            SetColdownText(_mineTimerCycle - _mineTimer);
        }
    }
    
    public void SetCycleMining(float newTimeCycle) => _mineTimerCycle = newTimeCycle;
    private void FinishMining(string tag) => Work?.Invoke(tag);
    private void SetColdownText(float time) => _coldownTimeText.text = time.ToString("#");
    private void ProgressFillAmount(float currentTime) => _progress.fillAmount = currentTime / _mineTimerCycle;

    private void AnimationMineText()
    {
        if (!_isAnimation) return;

        if (!_mineCountText.gameObject.activeInHierarchy)
        {
            _mineCountText.gameObject.SetActive(true);
        }

        _positionText += Time.deltaTime;
        _startTimeAnimation += Time.deltaTime;

        _mineCountText.rectTransform.position = new Vector3(_mineCountText.rectTransform.position.x, _positionText);
        if (_startTimeAnimation >= _timeAnimation)
        {
            _isAnimation = false;
            _mineCountText.gameObject.SetActive(false);
            _mineCountText.rectTransform.position = _defaulPosition;
            _positionText = _mineCountText.rectTransform.position.y;
            _startTimeAnimation = 0;
        }
    }

    public void MineCanvas(bool show) => _inMineCanvas.gameObject.SetActive(show);
    public void MinerResourcesPerCycleToText(int count) => _mineCountText.text = $"+{count}";
}
