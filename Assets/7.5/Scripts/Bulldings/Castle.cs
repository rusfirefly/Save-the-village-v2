using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Castle : MonoBehaviour, IDamageable, ISelecteble
{
    public static event Action CastleAttaking;
    [SerializeField] private float _health;
    private float _fullHealth;
    [SerializeField] private GameObject _fire;

    [SerializeField] private Text _castleInfoText;
    [SerializeField] private Button _repeirButton;
    [SerializeField] private Material _outlineMaterial;
    private SpriteRenderer _spriteRender;
    private Material _default;

    private int _countWoodForRepairPrice = 100;

    private float _timeRepair = 10f;
    private float _currentTime;
    private float _percentHealth;
    private Animator _animator;
    private bool isSelected;

    public void Start()
    {
        SetDefaultMaterial();
        _repeirButton.onClick += RepearCastle;///доелатьььь!!!!
        _fullHealth = _health;
    }

    public void TakeDamage(float damage)
    {
        OnCastleAttaking();
        _health -= damage;
        _percentHealth = _health / _fullHealth * 100;
        _castleInfoText.text = $"Замок:\n{_percentHealth}%";

        if (_percentHealth <= 80f)
            CasetleOnFire();

        if (_health <= 0)
        {
            GameOver();
        }
    }
    
    private void OnCastleAttaking()=> CastleAttaking?.Invoke();

    private void GameOver()
    {
        Debug.Log("Game Over!");
        SetBoolAnimation("IsDestroed", true);
    }

    private void OnMouseDown()
    {
        Selectet();
    }
    public bool IsSelected() => isSelected;
    private void CasetleOnFire()
    {
        _fire.gameObject.SetActive(true);
    }

    private void OnValidate()
    {
        GetAniamtion();
    }

    private void GetAniamtion() => _animator ??= GetComponent<Animator>();

    public void SetTriggerAnimation(string animation) => _animator.SetTrigger(animation);

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);

    private void RepearCastle()
    {

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
        _repeirButton.gameObject.SetActive(true);
    }

    private void HideRepairButton()
    {
        _repeirButton.gameObject.SetActive(false);
    }
    private void SetDefaultMaterial()
    {
        _spriteRender = gameObject.GetComponent<SpriteRenderer>();
        _default = _spriteRender.material;

    }
}
