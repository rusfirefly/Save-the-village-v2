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
    [SerializeField] private GameObject _repairBarPanel;
    [SerializeField] private Image _repairBar;
    private SpriteRenderer _spriteRender;
    private Material _default;

    [SerializeField] private int _countWoodForRepairPrice = 100;
    private SoundCastle _soundCastle;

    private float _timeRepair = 10f;
    private float _minHeathOnFire = 80f;
    private bool _isRepair;
    private bool _isFire;
    private bool _isAttacking;
    private float _currentTime;
    private float _percentHealth;
    private Animator _animator;
    
    public void Start()
    {
        SetDefaultMaterial();
        CreateListenerEvent();
        SetFullHealth();
        SetCastleHealth();
        SetRepairText();
        GetSoundComponent();
    }

    public void Reload()
    {
        SetDefauldMaterial();
        SetBoolAnimation("IsDestroed", false);
        SetCurrentHealthFull();
        SetCastleFire(false);
        SetCastleHealth();
    }

    private void GetSoundComponent()=> _soundCastle = gameObject.GetComponent<SoundCastle>();
    
    private void SetCastleHealth()
    {
        _percentHealth = _health / _fullHealth * 100;
        _castleInfoText.text = $"Замок:\nHP:{_percentHealth:F0}%";
    }

    private void SetDefauldMaterial() => _spriteRender.material = _default;
    
    private void SetCurrentHealthFull() => _health = _fullHealth;

    private void SetFullHealth() => _fullHealth = _health;

    private void SetRepairText() => _repairPriceText.text = $"Ремонт:\n{_countWoodForRepairPrice}";

    private void CreateListenerEvent() => _repairButton.onClick.AddListener(RepearCastle);

    public void TakeDamage(float damage)
    {
        _health -= damage;
        OnCastleAttaking();
        SetCastleHealth();

        if (_percentHealth <= _minHeathOnFire && !_isFire)
        {
            SetCastleFire(true);
            PlayeSoundCastleInFire();
        }

        IsGameOver();
    }

    private void PlayeSoundCastleInFire()=> _soundCastle.PlaySoundCastleInFire();

    private void Update()
    {
        CheckStateRepairButton();
        if (!_isRepair) return;
        RepairCastle();
    }

    private void RepairCastle()
    {
        _currentTime += Time.deltaTime;
        RepairBarFillAmount(_currentTime);
        if (_currentTime >= _timeRepair)
        {
            SetCurrentHealthFull();
            PlaySoundRepairComplete();
            _isAttacking = false;
            SetCastleHealth();
            SetCastleFire(false);
            RepairBarVisible(false);
            _currentTime -= _timeRepair;
            _isRepair = false;
        }
    }
    
    private void RepairBarVisible(bool visible)
    {
        _repairBar.fillAmount = 0;
        _repairBarPanel.SetActive(visible);
    }
    
    private void PlaySoundRepairComplete()=> _soundCastle.PlayeSoundCastleRepair();
    private void RepairBarFillAmount(float value)=> _repairBar.fillAmount = value / _timeRepair;

    private void CheckStateRepairButton()
    {
        if (PlayerBase.wood >= _countWoodForRepairPrice && _health < _fullHealth && !_isRepair)
        {
            RepairButtonEnable(true);
            ActiveButtonVisible(false);
        }
        else
        {
            RepairButtonEnable(false);
            ActiveButtonVisible(true);
        }
    }

    private void RepairButtonEnable(bool state)=> _repairButton.enabled = state;
    private void ActiveButtonVisible(bool visible)=>_activeButton.gameObject.SetActive(visible);

    private void OnCastleAttaking()
    {
        if (!_isAttacking)
        {
            PlaySoundCatleInFire();
            SetInAttack();
        }

        Attacked?.Invoke();
    }
    
    private void PlaySoundCatleInFire()=> _soundCastle.PlayeSoundWarning();
    private void SetInAttack()=> _isAttacking = true;
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
        Selected();
    }

    private void SetCastleFire(bool isFire)
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
        if(PlayerBase.wood>=_countWoodForRepairPrice)
        {
            PorgressBarVisible(true);
            SpendWoodOnRepairs();
            _isRepair = true;
        }else
        {
            PlayeSoundNeedWood();
        }
    }

    private void PlayeSoundNeedWood()=> _soundCastle.PlayeSoundNeedWood();
    private void SpendWoodOnRepairs() => PlayerBase.wood -= _countWoodForRepairPrice;
    
    private void PorgressBarVisible(bool visible)=> _repairBarPanel.SetActive(visible);

    public void Selected()
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
