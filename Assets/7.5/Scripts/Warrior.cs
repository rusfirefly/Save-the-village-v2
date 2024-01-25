using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Warrior : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Text _countKnightText;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRange = 0.5f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _attackSpeed = 0.5f;
    [SerializeField] private bool _drawGizmo;
    private float _nextAttackTime;
    [SerializeField] private Transform _tagetPosition;

    [SerializeField] private float _countInStek;
    private float _countStekHealth;
    [SerializeField] private int _health = 1;
    [SerializeField] private int _attack = 1;
    [SerializeField] private int _defence = 1;
    [SerializeField] private bool _isRun = true;
    private float _speedWarrior;
    private float _stepWarrior = 2;
    public bool firstWarrior { get; set; }

    private void Awake()
    {
        FindNavMeshAgent();
        SetupNavMeshAgent();
        MoveTo();
        SetCountStek();
    }

    private void Update()
    {
        Enemy enemy = FindEnemy();
        if (enemy!=null)
        {
            MoveToEnemy(enemy);
        }
        else
        {
            GoBackPosition();
        }

        AttackEnemy();
    }

    private void OnValidate()
    {
        SetCountStek();
        GetAniamtion();
    }
    private void OnDrawGizmos()
    {
        if (_attackPoint == null || !IsDrawGizmo()) return;
        DrawAttackRange();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UpdateStek(collision);
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

    private void MoveToEnemy(Enemy enemy)
    {
        float dist = GetDistanceToEnmy(enemy);
        if (CheckDistanceToEnemy(dist, distance: 3))
        {
            GoToEnemy(enemy);
        }
    }

    private bool CheckDistanceToEnemy(float currentDistance, float distance)
    {
        if (currentDistance <= distance) return true;
        else return false;
    }

    private Enemy FindEnemy()=> GameObject.FindObjectOfType<Enemy>();

    private void GoToEnemy(Enemy enemy)
    {
        _speedWarrior = _stepWarrior * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, enemy.gameObject.transform.position, _speedWarrior);
    }

    private float GetDistanceToEnmy(Enemy enemy)
    {
        return Vector2.Distance(gameObject.transform.position, enemy.gameObject.transform.position);
    }

    private void GoBackPosition()=> _agent.destination = _tagetPosition.position;


    private void AttackEnemy()
    {
        RunAgent();
        Collider2D[] hitEnemies = HitEnemys();
        foreach (Collider2D enemy in hitEnemies)
        {
            StopAgent();
            Attack(enemy);
        }
    }

    private Collider2D[] HitEnemys() => Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _layerMask);

    private void StopAgent()
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
        if (!_isRun)
        {
         _agent.enabled = true;
         _agent.isStopped = false;
         _isRun = true;
        }
    }

    private void Attack(Collider2D enemy)
    {
        _nextAttackTime += Time.deltaTime;
        if (_nextAttackTime >= _attackSpeed)
        {
            SetTriggerAnimation("Attack1");
            enemy.GetComponent<Enemy>().TakeDamage(_attack * _countInStek);
            _nextAttackTime = 0;
        }
    }
    
    private void GetAniamtion()=> _animator ??= GetComponent<Animator>();

    private bool IsDrawGizmo() => _drawGizmo;

    public void IncrementStek()
    {
        _countInStek++;
        _countKnightText.text = _countInStek.ToString("#");
    }

    public void UpdateCharater(int hp, int def, int atk, int countStek)
    {
        _countInStek = countStek;
        _health = hp;
        _attack = atk;
        _defence = def;

        SetCountStek();
    }

    private void SetCountStek()
    {
        if (_countKnightText == null) return;
        _countStekHealth = _countInStek * _health;
        if (_countInStek < 0) _countKnightText.text = "0";
        _countKnightText.text = _countInStek.ToString("#");
    }

    private void Die()
    {
        SetBoolAnimation("IsDie", true);
        GetComponent<Collider2D>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        DisableScript();
    }

    public void TakeDamage(float damage)
    {
        SetTriggerAnimation("Hit");
        if (damage <= 0) return;
        _countInStek -= CalculateDamage(damage);
        SetCountStek();
        if (_countInStek <= 0)
        {
            Die();
        }
    }
    
    private float CalculateDamage(float damage)
    {
        if (damage > _defence * _countInStek)
           return (damage - (_defence * _countInStek)) / _health;
        else
           return ((_defence * _countInStek) - damage) / _health;
    }

    public void DestroyEnemy() => Destroy(gameObject);

    public int GetDefence() => _defence;

    private void DrawAttackRange() => Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);

    private void DisableScript() => this.enabled = false;

    public void SetTriggerAnimation(string animation)=> _animator.SetTrigger(animation);

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);
    
    private void MoveTo()
    {
        if(_isRun && _tagetPosition!=null)
            _agent.destination = _tagetPosition.position;
    }

    public void GoToNewTargetPosition(Transform newPosition)
    {
        _tagetPosition = newPosition;
        MoveTo();
    }

    public void FindEnemyPosition()
    {
        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
            _agent.destination = enemy.gameObject.transform.position;
    }

    private void UpdateStek(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Warrior>().IncrementStek();

            if (!firstWarrior)
                Destroy(gameObject);
        }
    }
}
