using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Warrior : Entity, IDamageable, IMovable, IAttack
{
    public static event Action Deathing;

    [SerializeField] private float _baffAttack;
    [SerializeField] private float _baffDefence;
    [SerializeField] private SoundClip _needMeat;

   
    private float _satiety;

    protected override void Awake()
    {
        base.Awake();
        PlaySoundNewEntity();
    }

    private void Update()
    {
        Enemy enemy = FindEnemy();
        if (enemy != null)
        {
            MoveToEnemy(enemy);
        }
        else
        {
            GoBackPosition();
        }

        DetectHitEntity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DetectHitEntity();
    }

    protected override void Die()
    {
        base.Die();
        Deathing?.Invoke();
        DisableScript();
    }

    private void DetectHitEntity()
    {
        Collider2D[] hitEntities = HitEntity();
        foreach (Collider2D entity in hitEntities)
        {
            StopAgent();
            Attack(entity);
        }
    }

    private void MoveToEnemy(Enemy enemy)
    {
        float dist = GetDistanceToEnmy(enemy);
        if (CheckDistanceToEnemy(dist, _distanceFindEntity))
        {
            GoToEnemy(enemy);
        }
    }

    private bool CheckDistanceToEnemy(float currentDistance, float distance)
    {
        if (currentDistance <= distance) return true;
        else return false;
    }

    private Enemy FindEnemy() => FindObjectOfType<Enemy>();

    private void GoToEnemy(Enemy enemy)
    {
        _speedEntity = _stepEntity * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, enemy.gameObject.transform.position, _speedEntity);
    }

    private float GetDistanceToEnmy(Enemy enemy)
    {
        return Vector2.Distance(gameObject.transform.position, enemy.gameObject.transform.position);
    }

    public void Attack(Collider2D unit)
    {
        _nextAttackTime += Time.deltaTime;
        if (_nextAttackTime >= _attackSpeed)
        {
            SetTriggerAnimation("Attack1");
            unit.GetComponent<Enemy>().TakeDamage(_attack + _attack * _satiety/100);
            PlaySoundAttack();
            _nextAttackTime = 0;
        }
    }

    private void EatUp()
    {

    }
        

    private void Satiety()
    {
       //if(PlayerBase.meat)
    }

    public void TakeDamage(float damage)
    {
        SetTriggerAnimation("Hit");
        if (damage <= 0) return;
        _countInStek -= DamageÑalculation(damage);
        ViewCountStek();
        if (_countInStek <= 0)
        {
            Die();
        }
    }

   public void Move(Vector3 position)
    {
        if (_isRun && position != null)
        {
            _agent.destination = position;
        }
    }

    private float DamageÑalculation(float damage)
    {
        if (damage > _defence * _countInStek)
            return (damage - (_defence * _countInStek)) / _health;
        else
            return ((_defence * _countInStek) - damage) / _health;
    }

    public void GoToNewTargetPosition(Transform newPosition)
    {
        Vector3 newPoint = new Vector3(newPosition.position.x + (_random.Next(-5, 6) + 0.1f) * 0.6f, newPosition.position.y + (_random.Next(-2, 3) + 0.1f) * 0.3f);
        _tagetPosition = newPoint;
        Move(_tagetPosition);
    }

    public void FindEnemyPosition()
    {
        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
        {
            _agent.destination = enemy.gameObject.transform.position;
        }
    }

    public float GetPowerWarror() => _countInStek * (_attack + _defence + _health);
}
