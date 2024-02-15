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
    private Vector3 _defaulPosition;

    private void Start()
    {
        Initialize();

        GameManager.ReloadAll += OnReloadAll;
    }
    
    private void Update()
    {
        CycleDayAndNight();
    }

    private void OnDestroy()
    {
        GameManager.ReloadAll -= OnReloadAll;
    }
    
    public void Initialize()
    {
        _positionStartX = transform.localPosition.x;
        _defaulPosition = transform.localPosition;
        _positionEndX = _positionNewDay;
        _isStart = true;
        _currentDay = 1;
    }
    
    public void Reload()
    {
        _currentDay = 1;
        transform.localPosition = _defaulPosition;
    }

    public void SetNewSpeedTime(int newSpeedTime)
    {
        _speedTime = newSpeedTime;
    }

    private void OnReloadAll()
    {
        Reload();
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


}
