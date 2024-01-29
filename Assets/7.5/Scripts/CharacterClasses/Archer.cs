using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Archer : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Text _countStekText;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private bool _drawGizmo;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _attackSpeed = 0.5f;
    
    private float _nextAttackTime;
    [SerializeField] private Transform _tagetPosition;
    [SerializeField] private Transform _enemyPosition;

    [SerializeField] private float _countInStek;
    private float _countStekHealth;
    [SerializeField] private int _health = 1;
    [SerializeField] private int _attack = 1;
    [SerializeField] private int _defence = 1;
    [SerializeField] private bool _isRun = true;
    [SerializeField] private GameObject _arrowPrefab;
    private float _speedWarrior;
    private float _stepWarrior = 2;
     private float _distanceFindEnemy;
    public bool firstArcher { get; set; }
    private bool _isTargetInRange;

    private void Start()
    {
        FindNavMeshAgent();
        SetupNavMeshAgent();
        _distanceFindEnemy = _attackRange;
    }

    private void Update()
    {
        AttackEnemy();
    }

    private void OnDrawGizmos()
    {
        if (_attackPoint == null || !IsDrawGizmo()) return;
        DrawAttackRange();
        DrawArrowAttack();
     }

    private void OnValidate()
    {
        GetAnimation();
    }
    private void FindNavMeshAgent()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void SetupNavMeshAgent()
    {
        if (_agent == null) return;
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }
    private void GetAnimation() => _animator ??= GetComponent<Animator>();
    private void AttackEnemy()
    {
        Collider2D[] hitEnemies = HitEnemys();
        if(hitEnemies.Length>0) _isTargetInRange = true;
        else _isTargetInRange = false;
        foreach (Collider2D enemy in hitEnemies)
        {
            Arrow arrow = Instantiate(_arrowPrefab, _attackPoint.position, Quaternion.identity).GetComponent<Arrow>();
            _enemyPosition = enemy.transform;
            arrow.ShootArrow(enemy.transform);
        }
    }

    private void Shoot()
    {
        //анимация
    }
    private bool IsDrawGizmo() => _drawGizmo;
    private void DrawAttackRange() => Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    private Collider2D[] HitEnemys() => Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _layerMask);
    private void DrawArrowAttack()
    {
        if(_isTargetInRange && _enemyPosition!=null)
            Gizmos.DrawLine(_attackPoint.position, _enemyPosition.position);
    }
}
