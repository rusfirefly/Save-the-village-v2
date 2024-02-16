using UnityEngine;
using Random = System.Random;
/// <summary>
/// производство мяса =)))
/// </summary>
public class Miratorg : Mining
{
    private float _timeRandom;
    private float _currentTime;
    private int _minIntervalTime = 4;
    private int _maxIntervalTime = 5;

    private Random _random;
    [SerializeField] private AudioClip _audioSheep;

    private void Awake()
    {
        SetCycleMining(_gameSetup.timeMeatMine);
    }

    protected override void Start()
    {
        base.Start();
        _random = new Random();
        TimeRandomNext();
    }

    protected override void Update()
    {
        base.Update();
        RandomSheepSound();
    }

    private void TimeRandomNext() => _timeRandom = _random.Next(_minIntervalTime, _maxIntervalTime);

    private void RandomSheepSound()
    {
        if (GameMenu.isPaused) return;
        _currentTime += Time.deltaTime;
        if(_currentTime>=_timeRandom)
        {
            SheepSoundPlay();
            TimeRandomNext();
            _currentTime -= _timeRandom;
        }
    }

    private void SheepSoundPlay()
    {
        _soundClip.PlaySound(_audioSheep);
    }
}
