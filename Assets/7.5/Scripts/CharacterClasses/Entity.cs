using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour
{
    private SoundEntity _soundEntity;

    [SerializeField] private Animator _animator;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Transform _attackPoint;
    [SerializeField] protected float _attackRange = 0.5f;
    [SerializeField] private Image _healthImage;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected float _attackSpeed = 0.5f;
    [SerializeField] private bool _drawGizmo;
    [SerializeField] protected Image _hpBar;
    protected float _nextAttackTime;
    protected Vector3 _tagetPosition;

    [SerializeField] private Enums.TypeEntity _typeEntity;

    [SerializeField] protected float _health = 1;
    [SerializeField] protected float _healthFull = 1;
    [SerializeField] protected int _attack = 1;
    [SerializeField] protected int _defence = 1;
    [SerializeField] protected bool _isRun = true;
    private SpriteRenderer _spriteRander;
    protected float _speedEntity;
    protected float _stepEntity = 0.5f;

    [SerializeField] private Canvas _hud;
    [SerializeField] protected int _distanceFindEntity = 5;

    protected Random _random;
    protected bool _isDie;

    protected virtual void Awake()
    {
        FindNavMeshAgent();
        SetupNavMeshAgent();
        GetSoundEntity();
        _spriteRander = gameObject.GetComponent<SpriteRenderer>();
        _random = new Random();
        _agent.autoBraking = true;
    }

    private void OnValidate()
    {
        GetAniamtion();
    }

    protected virtual void OnDrawGizmos()
    {
        if (_attackPoint == null || !IsDrawGizmo()) return;
        DrawAttackRange();
    }
  
    private void OnTriggerExit2D(Collider2D collision)
    {
        RunAgent();
    }

    public void SetNewLayer(int layerLevel)
    {
        _spriteRander.sortingOrder = layerLevel;
        _hud.sortingOrder = layerLevel;
    }
    public Enums.TypeEntity GetEntityType() => _typeEntity;
    protected void GetSoundEntity() => _soundEntity = gameObject.GetComponent<SoundEntity>();
    protected void PlaySoundNewEntity() => _soundEntity.PlaySoundNewEntity();
    protected void PlaySoundAttack() => _soundEntity.PlaySoundAttack();
    protected void PlaySoundDie() => _soundEntity.PlaySoundDeath();

    private void FindNavMeshAgent()
    {
        _agent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void SetupNavMeshAgent()
    {
        if (_agent == null) return;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    protected void GoBackPosition()
    {
        RunAgent();
    }

    protected void HealthBarAmountFillAmount(float value) => _hpBar.fillAmount = value / (_healthFull);

    protected Collider2D[] HitEntity() => Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _layerMask);

    protected void StopAgent()
    {
        if (_isRun)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
            _isRun = false;
        }
    }

    private void RunAgent()
    {
        if (!_isRun&&!_isDie)
        {
            _agent.enabled = true;
            _agent.isStopped = false;
            if(!_agent.hasPath)
                _agent.destination = _tagetPosition;
            _isRun = true;
        }
    }


    private void GetAniamtion() => _animator ??= gameObject.GetComponent<Animator>();

    private bool IsDrawGizmo() => _drawGizmo;

    public void UpdateCharater(int hp, int def, int atk, int countStek)
    {
        _health = hp;
        _healthFull = hp;
        _attack = atk;
        _defence = def;
    }

    protected virtual void Die()
    {
        _isDie = true;
        _agent.enabled = false;
        PlaySoundDie();
        SetBoolAnimation("IsDie", true);
        
        gameObject.GetComponent<Collider2D>().enabled = false;
    }

    public void DestroyEntity() => Destroy(gameObject);

    public int GetDefence() => _defence;

    private void DrawAttackRange() => Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);

    protected void DisableScript() => this.enabled = false;

    public void SetTriggerAnimation(string animation) => _animator.SetTrigger(animation);

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);
}
