using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]

public class House : MonoBehaviour,ISelecteble
{
    public static event Action BuildComplete;
    public static event Action<Vector3> NeedEngineer;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Text _priceText;
    private Material _default;
    private SpriteRenderer _spriteRender;
    private SoundClip _sound;
    private bool _isSelected;
    private bool _isBulding;
    private bool _isComplete;
    private float _currentTime;
    [SerializeField] private float _timeBulding;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _needWoodSound;
    [SerializeField] private AudioClip _buildingComplete;
    [SerializeField] private Canvas _canvasPrice;
    [SerializeField] private Canvas _canvasProgress;
    [SerializeField] private int _priceBuilding;
    [SerializeField] private Text _progressBuildingText;
    private WorkManEngineer unit;
    

    private void Start()
    {
        _sound = gameObject.GetComponent<SoundClip>();
        _priceText.text = $"{_priceBuilding}";
    }

    private void Update()
    {
        BuldingHouse();
    }

    private void OnValidate()
    {
        _animator ??= gameObject.GetComponent<Animator>();
        _priceText.text = $"{_priceBuilding}";
    }
    private void BuldingHouse()
    {
        if (_isComplete) return;
        if (!_isBulding) return;
        _currentTime += Time.deltaTime;
        _progressBuildingText.text = $"{(_timeBulding - _currentTime):F0}s";
        if (_currentTime>=_timeBulding)
        {
            _canvasPrice.gameObject.SetActive(false);
            _canvasProgress.gameObject.SetActive(false);
            _animator.SetBool("CompleteBuild", true);
            _sound.PlaySound(_buildingComplete);
            unit.StopWork();
            BuildComplete?.Invoke();
            _currentTime -= _timeBulding;
            _isComplete = true;
            _isBulding = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "worker"&& !_isBulding && _isSelected)
        {
            _isBulding = true;
            _canvasPrice.gameObject.SetActive(false);
            _canvasProgress.gameObject.SetActive(true);
            _progressBuildingText.gameObject.SetActive(true);
            unit = collision.gameObject.GetComponent<WorkManEngineer>();
            unit.StartWork();
        }
    }

    private void OnMouseDown()
    {
        if (WorkManEngineer.CountWork == Population.EngineerCountTotal) return;
        if (Storage.Wood < _priceBuilding)
        {
            _sound.PlaySound(_needWoodSound);
            return;
        }

        if (!_isBulding)
        {
            //_isBulding = true;
            _isSelected = true;
            NeedEngineer?.Invoke(gameObject.transform.position);
        }
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
            _spriteRender.material = _outlineMaterial;
            SelectedBuilding.OnSelected(gameObject);
        }
    }

    public void Reload()
    {
        _canvasPrice.gameObject.SetActive(true);
        _canvasProgress.gameObject.SetActive(false);
        _animator.SetBool("CompleteBuild", false);
        _isComplete = false;
    }

}
