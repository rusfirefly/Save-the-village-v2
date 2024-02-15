using System;
using UnityEngine;

public class DayAndNight : MonoBehaviour
{
    public static event Action<int> NewDay;
    [SerializeField] private float _speedTime;

    private float _positionEndX;
    private float _positionStartX;
    private const float _positionNewDay = -62f;
    private bool _isStart;
    private int _currentDay;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _positionStartX = transform.localPosition.x;
        _positionEndX = _positionNewDay;
        _isStart = true;
        _currentDay = 1;
    }

    private void CycleDayAndNight()
    {
        if (GameMenu.isPaused) return;
        if (_isStart)
        {
            if (transform.localPosition.x <= _positionEndX)
            {
                transform.localPosition = new Vector3(_positionStartX, transform.localPosition.y, 0);
                _currentDay++;
                NewDay?.Invoke(_currentDay);
            }

            transform.Translate(Vector3.left * _speedTime * Time.deltaTime);
        }
    }

    private void Update()
    {
         CycleDayAndNight();
    }

    public void Reload()
    {
        _isStart = false;
        _currentDay = 1;
    }

    public void SetNewSpeedTime(int newSpeedTime)
    {
        _speedTime = newSpeedTime;
    }
}
