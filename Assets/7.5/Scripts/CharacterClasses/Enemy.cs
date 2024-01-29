using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IDamageable, IMovable, IAttack
{
    public enum TypeEnym {Knight, TNT, Torch};
    [SerializeField] private TypeEnym _typeEnym;
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
    private float _speedEnemy;
    private float _stepEnemy = 2;
    public bool firstEnemy;
    private int _distance = 2;

    private void Awake()
    {
        FindNavMeshAgent();
        SetupNavMeshAgent();
    }

    private void Update()
    {
        Warrior playerWarrior = FindWarrior();

        if (playerWarrior != null)
        {
            MoveToWarrior(playerWarrior);
        }
        else
        {
            if(GetDistanceTo(_tagetPosition)>2)
                GoBackPosition();
        }

        AttackWarrior();
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

    
    private void MoveToWarrior(Warrior warrior)
    {
        float dist = GetDistanceToWarrior(warrior);
        if (CheckDistanceToWarrior(dist, _distance))
        {
            RunToWarrior(warrior);
        }
    }

    public TypeEnym GetTypeEnemy() => _typeEnym;

    private bool CheckDistanceToWarrior(float currentDistance, float distance)
    {
        if (currentDistance <= distance) return true;
        else return false;
    }

    private Warrior FindWarrior() => GameObject.FindObjectOfType<Warrior>();

    private void RunToWarrior(Warrior warrior)
    {
        _speedEnemy = _stepEnemy * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, warrior.gameObject.transform.position, _speedEnemy);
    }

    private float GetDistanceToWarrior(Warrior warrior)
    {
        return Vector2.Distance(gameObject.transform.position, warrior.gameObject.transform.position);
    }

    private float GetDistanceTo(Transform tagetPosition)
    {
        return Vector2.Distance(gameObject.transform.position, tagetPosition.gameObject.transform.position);
    }


    private void GoBackPosition()
    {
         _agent.destination = _tagetPosition.position;
    }


    private void AttackWarrior()
    {
        
        Collider2D[] hitWarriors = HitWarriors();

        if (hitWarriors.Length == 0)
            RunAgent();

        foreach (Collider2D warrior in hitWarriors)
        {
             StopAgent();
             Attack(warrior);
        }
    }

    private Collider2D[] HitWarriors() => Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _layerMask);

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

    public void Attack(Collider2D unit)
    {
        _nextAttackTime += Time.deltaTime;
        if (_nextAttackTime >= _attackSpeed)
        {
            StartAnimationAttack();
            TakeDamage(unit);
            _nextAttackTime = 0;
        }
    }

    private void TakeDamage(Collider2D unit)
    {
        if (unit.gameObject.tag == "Castle")
            unit.GetComponent<Castle>().TakeDamage(_attack * _countInStek);
        else
            unit.GetComponent<Warrior>().TakeDamage(_attack * _countInStek);

        if (_typeEnym == TypeEnym.TNT)
        {
            TakeDamage(_attack * _countInStek);
        }
    }

    private void StartAnimationAttack()
    {
        if (_typeEnym == TypeEnym.TNT)
        {
            SetTriggerAnimation("IsExplosion");
        }
        else
            SetTriggerAnimation("Attack1");
    }

    private void GetAniamtion() => _animator ??= GetComponent<Animator>();

    private bool IsDrawGizmo() => _drawGizmo;

    public void IncrementStek(float value)
    {
        _countInStek+= value;
        _countKnightText.text = _countInStek.ToString("#");
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
        if (_typeEnym != TypeEnym.TNT)
            SetTriggerAnimation("Hit");

        if (damage <= 0) return;

        _countInStek -= CalculateDamage(damage);
        SetCountStek();
        if (_countInStek <= 0)
        {
           Die();
        }
    }
    public void InitEnemy(int hp, int def, int atk, int countStek)
    {
        _countInStek = countStek;
        _health = hp;
        _attack = atk;
        _defence = def;

        SetCountStek();
        Move(_tagetPosition);
    }

    public void InitEnemy(int countStek)
    {
        if (_typeEnym == TypeEnym.TNT)
            _countInStek = 1;
        else
            _countInStek = countStek;

        SetCountStek();
        Move(_tagetPosition);
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

    public void SetTriggerAnimation(string animation) => _animator.SetTrigger(animation);

    private void SetBoolAnimation(string animation, bool value) => _animator.SetBool(animation, value);

    public void Move(Transform position)
    {
        if (_isRun && position != null)
            _agent.destination = position.position;
    }

    public void GoToNewTargetPosition(Transform newPosition)
    {
        _tagetPosition = newPosition;
        Move(_tagetPosition);
    }

    public void FindEnemyPosition()
    {
        Enemy[] enemys = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
            _agent.destination = enemy.gameObject.transform.position;
    }

    private void UpdateStek(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy collisionEnemy = collision.gameObject.GetComponent<Enemy>();

            if (collisionEnemy._typeEnym == _typeEnym)
            {
                collisionEnemy.IncrementStek(GetStek());
                if (!firstEnemy)
                    Destroy(gameObject);
            }
        }
    }

    private float GetStek() => _countInStek;
    public void SetTargetPosition(Transform newPosition) => _tagetPosition = newPosition;

    public float GetPowerEnemy() => _countInStek * (_attack + _defence + _health);
}
