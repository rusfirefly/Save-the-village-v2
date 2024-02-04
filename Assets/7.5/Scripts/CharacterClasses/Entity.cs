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
    [SerializeField] private Text _countStekText;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private Image _healthImage;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] protected float _attackSpeed = 0.5f;
    [SerializeField] private bool _drawGizmo;

    protected float _nextAttackTime;
    protected Vector3 _tagetPosition;

    [SerializeField] protected float _countInStek;
    [SerializeField] protected int _health = 1;
    [SerializeField] protected int _attack = 1;
    [SerializeField] protected int _defence = 1;
    [SerializeField] protected bool _isRun = true;
    private SpriteRenderer _spriteRander;
    protected float _speedEntity;
    protected float _stepEntity = 2;

    [SerializeField] protected int _distanceFindEntity = 3;

    protected Random _random;

    protected virtual void Awake()
    {
        FindNavMeshAgent();
        SetupNavMeshAgent();
        ViewCountStek();
        GetSoundEntity();
        _spriteRander = gameObject.GetComponent<SpriteRenderer>();
        _random = new Random();
    }

    private void OnValidate()
    {
        ViewCountStek();
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
    }

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

    protected Collider2D[] HitEntity() => Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _layerMask);

    protected void StopAgent()
    {
        if (_isRun)
        {
            if(_agent.isOnNavMesh)
                _agent.isStopped = true;

            _isRun = false;
        }
    }

    private void RunAgent()
    {
        if (!_isRun)
        {
            if (_agent.isOnNavMesh)
            {
                _agent.isStopped = false;
                _agent.destination = _tagetPosition;
            }

            _isRun = true;
        }
    }


    private void GetAniamtion() => _animator ??= gameObject.GetComponent<Animator>();

    private bool IsDrawGizmo() => _drawGizmo;

    public void IncrementStek()
    {
        _countInStek++;
        _countStekText.text = _countInStek.ToString("#");
    }

    public void UpdateCharater(int hp, int def, int atk, int countStek)
    {
        _countInStek = countStek;
        _health = hp;
        _attack = atk;
        _defence = def;

        ViewCountStek();
    }

    protected void ViewCountStek()
    {
        if (_countStekText == null) return;
        if (_countInStek <= 0) _countInStek = 0;
        _countStekText.text = _countInStek.ToString("#.#");
    }

    protected virtual void Die()
    {
        PlaySoundDie();
        SetBoolAnimation("IsDie", true);
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<NavMeshAgent>().enabled = false;
    }

    public void DestroyEntity() => Destroy(gameObject);

    public int GetDefence() => _defence;

    private void DrawAttackRange() => Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);

    protected void DisableScript() => this.enabled = false;

    public void SetTriggerAnimation(string animation) => _animator.SetTrigger(animation);

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);
}
