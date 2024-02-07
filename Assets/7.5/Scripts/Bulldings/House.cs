using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class House : MonoBehaviour,ISelecteble
{
    public static event Action BuildComplete;
    [SerializeField] private Material _outlineMaterial;
    private Material _default;
    private SpriteRenderer _spriteRender;
    private SoundClip _sound;
    private bool _isSelected;
    private bool _isBulding;
    private bool _isComplete;
    private float _currentTime;
    [SerializeField] private float _timeBulding;

    private void Start()
    {
        _sound = gameObject.GetComponent<SoundClip>();

    }

    private void Update()
    {
        BuldingHouse();
    }

    private void BuldingHouse()
    {
        if (_isComplete) return;
        if (!_isBulding) return;
        _currentTime += Time.deltaTime;
        Debug.Log($"{(_timeBulding - _currentTime):F0}");
        if(_currentTime>=_timeBulding)
        {
            //построить
            //увеличить кол-во воинов и рабочих
            BuildComplete?.Invoke();
            _currentTime -= _timeBulding;
            _isComplete = true;
            _isBulding = false;
        }
    }

    private void OnMouseDown()
    {
        if(!_isBulding)
            _isBulding = true;
    }

    public void DeSelected()
    {
        _isSelected = false;
        _spriteRender.material = _default;
        SelectedBuilding.selectedObject = null;
    }

    public void Selected()
    {
        if (!_isSelected)
        {
            _isSelected = true;
            _sound.PlaySound();
            _spriteRender.material = _outlineMaterial;
            SelectedBuilding.OnSelected(gameObject);
        }
    }

    public void Reload()
    {
        _isComplete = false;
    }

}
