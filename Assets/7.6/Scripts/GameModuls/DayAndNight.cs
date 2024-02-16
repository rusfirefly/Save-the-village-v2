using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour
{
    public static event Action<int> NewDay;

    [SerializeField] private float _speedTime;
    [SerializeField] private float _currentTime;
    [SerializeField] private SpriteRenderer _dayNightSpriteRender;
    [SerializeField] private Text _currentTimeText;

    private bool _isStart;
    private int _currentDay;
    private bool _isNewDay;

    private const float _timeKoeficient = 0.06f;
    private const float _dayDuration = 24f;
    private const float _seconds = 60f;
    private const float _eveningTime = 18f;
    private const float _morningTime = 6f;
    private const float _totalTime = 12f;

    private void OnDestroy()
    {
        GameHadler.ReloadAll -= OnReloadAll;
    }

    public void Initialize()
    {
        Reload();
        GameHadler.ReloadAll += OnReloadAll;
        StartCoroutine(CylcletDayNight());
    }

    public void UpdateAlphaBasedOnTime(float time)
    {
        float alpha = Mathf.PingPong(CalculateFractionalValue(time),0.5f);
        Color color = _dayNightSpriteRender.color;
        color.a = alpha;
        _dayNightSpriteRender.color = color;
    }

    private IEnumerator CylcletDayNight()
    {
        if (GameMenu.isPaused == false)
        {
            if (_isStart)
            {
                _currentTime += _timeKoeficient;
                if (_currentTime >= _dayDuration)
                {
                    _currentTime = 0;
                    _isNewDay = true;
                }

                TimeSpan timeSpan = new TimeSpan(0, 0, Mathf.RoundToInt(_currentTime * _seconds));
                _currentTimeText.text = $"{timeSpan.Minutes}÷ {timeSpan.Seconds}ì";

                if (_currentTime >= 6.1f && _isNewDay)
                {
                    _currentDay++;
                    NewDay?.Invoke(_currentDay);
                    _isNewDay = false;
                }

                UpdateAlphaBasedOnTime(_currentTime);

                yield return new WaitForSeconds(_speedTime);
                StartCoroutine(CylcletDayNight());
            }
        }
        else
        {
            yield return new WaitForSeconds(_speedTime);
            StartCoroutine(CylcletDayNight());
        }
    }

    public void Reload()
    {
        _isStart = true;
        _currentDay = 1;
        NewDay?.Invoke(_currentDay);
    }

    public void SetNewSpeedTime(int newSpeedTime)
    {
        _speedTime = newSpeedTime;
    }


    private float CalculateFractionalValue(float hour)
    {
        float calculateValue = 0;
        if (hour >= _eveningTime && hour <= _dayDuration)
            calculateValue = (hour - _eveningTime) / _totalTime;
        else
            if (hour >= 0 && hour <= _morningTime)
            calculateValue = (hour - _morningTime) / _totalTime;

        return calculateValue;
    }


    private void OnReloadAll()
    {
        Reload();
    }
}
