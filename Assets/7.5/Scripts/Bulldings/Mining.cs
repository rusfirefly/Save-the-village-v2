using System;
using UnityEngine;
using UnityEngine.UI;


public class Mining : MonoBehaviour
{
    public static event Action<string> Work;
    public bool InMine { get; set; }

    [SerializeField] private Image _progress;
    [SerializeField] private Text _coldownTimeText;
    [SerializeField] private Text _mineCountText;
    [SerializeField] private Canvas _inMineCanvas;
    [SerializeField] private float _mineTimerCycle;
    [SerializeField] private int _countResources;
    [SerializeField] protected AudioClip _audioMiningFinish;
    protected SoundClip _soundClip; 

    private float _mineTimer;
    private bool _isAnimation;
    private float _positionText;
    private Vector3 _defaulPosition;

    private float _timeAnimation = 2f;
    private float _startTimeAnimation = 0;

    protected virtual void Start()
    {
        Reload();
        _defaulPosition = _mineCountText.rectTransform.position;
        _positionText = _mineCountText.rectTransform.position.y;
        _soundClip = gameObject.GetComponent<SoundClip>();
    }
    
    public void Reload()
    {
        InMine = false;
        _mineTimer = 0;
        _mineCountText.gameObject.SetActive(false);

        MineCanvas(false);
    }

    protected virtual void Update()
    {
        Mine();
        AnimationMineText();
    }

    private void Mine()
    {
        if (InMine)
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

    private void FinishMining(string tag)
    {
        PlaySoundMinig();
        Work?.Invoke(tag);
    }

    private void SetColdownText(float time) => _coldownTimeText.text = $"{time:F0}";
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

    protected virtual void PlaySoundMinig()=>_soundClip.PlaySound(_audioMiningFinish);
}
