using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Castle : MonoBehaviour, IDamageable, ISelecteble
{
    public static event Action Attacked;
    public static event Action Destroyed;
    [SerializeField] private float _health;
    private float _fullHealth;
    [SerializeField] private GameObject _fire;

    [SerializeField] private Text _castleInfoText;
    [SerializeField] private Text _repairPriceText;
    [SerializeField] private Button _repairButton;
    [SerializeField] private Image _activeButton;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private GameObject _progressBarPanel;
    [SerializeField] private Image _progressBar;
    private SpriteRenderer _spriteRender;
    private Material _default;

    [SerializeField] private int _countWoodForRepairPrice = 100;

    private float _timeRepair = 10f;
    private bool _isRepair;
    private bool _isFire;
    private float _currentTime;
    private float _percentHealth;
    private Animator _animator;
    private bool isSelected;

    public void Start()
    {
        SetDefaultMaterial();
        CreateListenerEvent();
        _fullHealth = _health;
        SetCastleHealth();
        SetRepairText();
    }
    
    private void SetCastleHealth()
    {
        _percentHealth = _health / _fullHealth * 100;
        Debug.Log(_percentHealth);
        _castleInfoText.text = $"Замок:\nHP:{Math.Round(_percentHealth)}%";
    }

    private void SetRepairText()=> _repairPriceText.text = $"Ремонт:\n{_countWoodForRepairPrice}";

    private void CreateListenerEvent()=> _repairButton.onClick.AddListener(RepearCastle);

    public void TakeDamage(float damage)
    {
        OnCastleAttaking();
        _health -= damage;
        
        SetCastleHealth();

        if (_percentHealth <= 80f)
        {
            SetCasetleFire(true);
        }

        IsGameOver();
    }

    private void Update()
    {
        if(PlayerBase.wood >= _countWoodForRepairPrice && _health < _fullHealth && !_isRepair)
        {
            _repairButton.enabled = true;
            _activeButton.gameObject.SetActive(false);
        }else
        {
            _repairButton.enabled = false;
            _activeButton.gameObject.SetActive(true);
        }

        if (!_isRepair) return;

        _currentTime += Time.deltaTime;
        _progressBar.fillAmount = _currentTime / _timeRepair;
        if (_currentTime>=_timeRepair)
        {
            _health = _fullHealth;
            //звук - починка завершена
            SetCastleHealth();
            SetCasetleFire(false);
            _progressBar.fillAmount = 0;
            _progressBarPanel.SetActive(false);
            _currentTime -= _timeRepair;
            _isRepair = false;
        }
    }

    private void OnCastleAttaking()=> Attacked?.Invoke();

    private void IsGameOver()
    {
        if (_health <= 0)
        {
            SetBoolAnimation("IsDestroed", true);
            Destroyed?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Selectet();
    }

    public bool IsSelected() => isSelected;

    private void SetCasetleFire(bool isFire)
    {
        _fire.gameObject.SetActive(isFire);
        _isFire = isFire;
    }

    private void OnValidate()
    {
        GetAnimation();
    }

    private void GetAnimation() => _animator ??= GetComponent<Animator>();

    public void SetTriggerAnimation(string animation) => _animator.SetTrigger(animation);

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);

    private void RepearCastle()
    {
        if(_health == _fullHealth)
        {
            //звук - ремонт не требуется
        }

        if(PlayerBase.wood>=_countWoodForRepairPrice)
        {
            _progressBarPanel.SetActive(true);
            PlayerBase.wood -= _countWoodForRepairPrice;
            _isRepair = true;
        }
    }

    public void Selectet()
    {
        _spriteRender.material = _outlineMaterial;
        ShowRepairButton();
        SelectedBuilding.OnSelected(gameObject);
    }

    public void DeSelected()
    {
        _spriteRender.material = _default;
        HideRepairButton();
        SelectedBuilding.selectedObject = null;
    }

    public void ShowRepairButton()
    {
        _repairButton.gameObject.SetActive(true);
        _repairPriceText.text = $"Ремонт:\n{_countWoodForRepairPrice}";
    }

    private void HideRepairButton()
    {
        _repairButton.gameObject.SetActive(false);
    }
    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;

    }
}
