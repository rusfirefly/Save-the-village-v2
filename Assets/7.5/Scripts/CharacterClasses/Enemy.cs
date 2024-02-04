using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TypeEnym = Enums.TypeEnym;

public class Enemy : Entity, IDamageable, IMovable, IAttack
{
    public static event Action Deathing;

    [SerializeField] private TypeEnym _typeEnym;

    private void Update()
    {
        Warrior playerWarrior = FindWarrior();

        if (playerWarrior != null)
        {
            MoveToWarrior(playerWarrior);
        }
        else
        {
            GoBackPosition();
        }

        DetectHitEntity();
    }

    protected override void Die()
    {
        base.Die();
        
        Deathing?.Invoke();
        DisableScript();
    }

    public TypeEnym GetTypeEnemy() => _typeEnym;

    private void MoveToWarrior(Warrior warrior)
    {
        float dist = GetDistanceToWarrior(warrior);
        if (CheckDistanceToWarrior(dist, _distanceFindEntity))
        {
           RunToWarrior(warrior);
        }
    }
    private bool CheckDistanceToWarrior(float currentDistance, float distance)
    {
        if (currentDistance <= distance) return true;
        else return false;
    }

    private Warrior FindWarrior() => GameObject.FindObjectOfType<Warrior>();

    private void RunToWarrior(Warrior warrior)
    {
       _speedEntity = _stepEntity * Time.deltaTime;
       gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, warrior.gameObject.transform.position, _speedEntity);
    }

    private float GetDistanceToWarrior(Warrior warrior)
    {
        return Vector2.Distance(gameObject.transform.position, warrior.gameObject.transform.position);
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

    public void Attack(Collider2D unit)
    {
        _nextAttackTime += Time.deltaTime;
        if (_nextAttackTime >= _attackSpeed)
        {
            StartAnimationAttack();
            TakeDamage(unit);
            PlaySoundAttack();
            if (_typeEnym == TypeEnym.TNT)
            {
                StopAgent();
            }
            _nextAttackTime = 0;
        }
    }
    
    private void TakeDamage(Collider2D unit)
    {
       
        if (unit.gameObject.tag == "Castle")
        {
            unit.GetComponent<Castle>().TakeDamage(_attack);
            _distanceFindEntity = 0;
        }
        else
        {
            unit.GetComponent<Warrior>().TakeDamage(_attack);
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

    public void TakeDamage(float damage)
    {
        if (_typeEnym != TypeEnym.TNT)
            SetTriggerAnimation("Hit");

        if (damage <= 0) return;

        _countInStek -= DamageÑalculation(damage);
     
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
        Move(_tagetPosition);
    }

    public void InitEnemy(int countStek)
    {
        _countInStek = countStek;
        Move(_tagetPosition);
    }

    private float DamageÑalculation(float damage)
    {
        if (damage > _defence * _countInStek)
            return (damage - (_defence * _countInStek)) / _health;
        else
            return ((_defence * _countInStek) - damage) / _health;
    }

    
    public void Move(Vector3 position)
    {
        if (_isRun && position != null)
            _agent.destination = position;
    }

    public void GoToNewTargetPosition(Transform newPosition)
    {
        _tagetPosition = newPosition.position;
        Move(_tagetPosition);
    }

    public void FindEnemyPosition()
    {
        Enemy[] enemys = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
            _agent.destination = enemy.gameObject.transform.position;
    }
    public void AgentDisable()
    {
        StopAgent();

    }

    public void SetTargetPosition(Transform newPosition) => _tagetPosition = newPosition.position;
}
