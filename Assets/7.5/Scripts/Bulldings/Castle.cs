using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Castle : MonoBehaviour, IDamageable, ISelecteble
{
    public static event Action<Vector3> Attacked;
    public static event Action Destroyed;
    public static event Action<int> Repair;

    [SerializeField] private float _health;
    [SerializeField] private GameObject _fire;
    [SerializeField] private Text _castleInfoText;
    [SerializeField] private Text _repairPriceText;
    [SerializeField] private Button _repairButton;
    [SerializeField] private Image _activeButton;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private GameObject _repairBarPanel;
    [SerializeField] private Image _repairBar;
    [SerializeField] private Transform _positionArcher;
    [SerializeField] private int _countWoodForRepairPrice = 100;

    [SerializeField] private PlayerData _playerData;
    private float _fullHealth;

    private SpriteRenderer _spriteRender;
    private Material _default;
    private SoundCastle _soundCastle;

    private float _timeRepair = 10f;
    private float _minHeathOnFire = 80f;
    private bool _isRepair;
    private bool _isFire;
    private bool _isAttacking;
    private float _currentTime;
    private float _percentHealth;
    private Animator _animator;
    private BuffSkill _buffSkill;
    private PlayerBase _playerBase;

    public void Start()
    {
        GetSoundComponent();
        SetDefaultMaterial();
        CreateListenerEvent();
        SetFullHealth();
        SetCastleHealth();
        SetRepairText();
        InitCastleSkill();

        CreatePlayerBase();
    }

    private void Update()
    {
        CastleBuff();
        CheckStateRepairButton();
        if (!_isRepair) return;
        RepairCastle();
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Selected();
    }

    private void OnValidate()
    {
        GetAnimation();
    }

    public void Reload()
    {
        DefaultValue();
        SetDefauldMaterial();
        SetBoolAnimation("IsDestroed", false);
        SetCurrentHealthFull();
        SetCastleFire(false);
        SetCastleHealth();
        RepairBarVisible(false);

        _playerBase.ReloadValue();
    }

    private void CreatePlayerBase()
    {
        _playerBase = new PlayerBase(_playerData, initGoldCount: 12, initMeatCount: 0, initWoodCount: 0);
    }

    private void GetAnimation() => _animator ??= GetComponent<Animator>();

    private void GetSoundComponent() => _soundCastle = gameObject.GetComponent<SoundCastle>();

    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;
    }

    private void CreateListenerEvent() => _repairButton.onClick.AddListener(RepearCastle);

    private void SetFullHealth() => _fullHealth = _health;

    private void SetCastleHealth()
    {
        _percentHealth = _health / _fullHealth * 100;
        _castleInfoText.text = $"Замок:\nHP:{_percentHealth:F0}%";
    }

    private void SetRepairText() => _repairPriceText.text = $"Ремонт:\n{_countWoodForRepairPrice}";

    private void InitCastleSkill()
    {
        _buffSkill = gameObject.GetComponent<BuffSkill>();
        _buffSkill.BuffVisible(false);
    }

    private void DefaultValue()
    {
        _isRepair = false;
        _currentTime = 0;
    }

    private void SetDefauldMaterial() => _spriteRender.material = _default;

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);

    private void SetCurrentHealthFull() => _health = _fullHealth;

    private void SetCastleFire(bool isFire)
    {
        _fire.gameObject.SetActive(isFire);
        _isFire = isFire;
    }

    private void RepairBarVisible(bool visible)
    {
        _repairBar.fillAmount = 0;
        _repairBarPanel.SetActive(visible);
    }

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

    public void SetTriggerAnimation(string animation) => _animator.SetTrigger(animation);

    private void PlayeSoundCastleInFire()=> _soundCastle.PlaySoundCastleInFire();

    private void CastleBuff()
    {
        if (Storage.Meat > 0)
        {
            _buffSkill.EnableBuff();
            BuffShowInHud();
        }
        else
        {
            _buffSkill.DisableBuff();
            _buffSkill.BuffVisible(false);
        }
    }

    private void BuffShowInHud() => _buffSkill.BuffVisible(true);

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
    
    private void PlaySoundRepairComplete()=> _soundCastle.PlayeSoundCastleRepair();

    private void RepairBarFillAmount(float value)=> _repairBar.fillAmount = value / _timeRepair;

    private void CheckStateRepairButton()
    {
        if (Storage.Wood >= _countWoodForRepairPrice && _health < _fullHealth && !_isRepair)
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

        Attacked?.Invoke(transform.position);
    }
    
    private void PlaySoundCatleInFire()=> _soundCastle.PlayeSoundWarning();

    private void SetInAttack()=> _isAttacking = true;

    private void IsGameOver()
    {
        if (_health <= 0)
        {
            DeathArcherInCastle();
            SetBoolAnimation("IsDestroed", true);
            Destroyed?.Invoke();
        }
    }

    private void DeathArcherInCastle()
    {
        Archer[] archers = FindObjectsOfType<Archer>();
        foreach (Archer archer in archers)
            archer.TakeDamage(1000);
    }

    private void RepearCastle()
    {
        if(Storage.Wood >= _countWoodForRepairPrice)
        {
            PorgressBarVisible(true);
            SpendWoodOnRepairs();
            _isRepair = true;
        }else
        {
            PlayeSoundNeedWood();
        }
    }

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
    public void ArcherOnCastle(Archer archer)
    {
        archer.transform.position = _positionArcher.position;
    }
    private void PlayeSoundNeedWood() => _soundCastle.PlayeSoundNeedWood();

    private void SpendWoodOnRepairs() => Repair?.Invoke(_countWoodForRepairPrice);

    private void PorgressBarVisible(bool visible) => _repairBarPanel.SetActive(visible);

    private void HideRepairButton()
    {
        _repairButton.gameObject.SetActive(false);
    }
}
