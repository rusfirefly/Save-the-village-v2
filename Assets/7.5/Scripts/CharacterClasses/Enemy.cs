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
        if (_isDie) return;
        DetectEntity();
        DetectHitEntity();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer & (1 << _layerMask.value)) == 0)
        {
            MoveToEntity(collision.gameObject.transform.position);
        }
        else
        {
            GoBackPosition();
        }
    }

    private void DetectEntity()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _distanceFindEntity, _layerMask);
        foreach (Collider2D collider in colliders)
        {
            MoveToEntity(collider.transform.position);
        }

        if (colliders.Length == 0)
        {
            GoBackPosition();
        }
    }

    protected override void Die()
    {
        base.Die();
        Deathing?.Invoke();
        DisableScript();
    }

    public TypeEnym GetTypeEnemy() => _typeEnym;

    private void MoveToEntity(Vector3 enemy)
    {
        float dist = GetDistanceToEntity(enemy);
        if (CheckDistanceToEntity(dist, _distanceFindEntity))
        {
            GoToEntity(enemy);
        }
    }
    private float GetDistanceToEntity(Vector3 enemyPosition)
    {
        return Vector2.Distance(gameObject.transform.position, enemyPosition);
    }

    private void GoToEntity(Vector3 enemyPosition)
    {
        _speedEntity = _stepEntity * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, enemyPosition, _speedEntity);
    }

    private bool CheckDistanceToEntity(float currentDistance, float distance)
    {
        if (currentDistance <= distance) return true;
        else return false;
    }

    private Entity FindEntity()
    {
        Entity entity = GameObject.FindObjectOfType<Entity>();
        if (entity.GetEntityType() == Enums.TypeEntity.Player)
            return entity;
        else return null;
    }

    private void RunToEntity(Entity entity)
    {
        _speedEntity = _stepEntity * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, entity.gameObject.transform.position, _speedEntity);
    }

    private float GetDistanceToEntity(Entity entity)
    {
        return Vector2.Distance(gameObject.transform.position, entity.gameObject.transform.position);
    }

    private void DetectHitEntity()
    {
        Collider2D[] hitEntities = HitEntity();
        //foreach (Collider2D entity in hitEntities)
        //{
        if (hitEntities.Length > 0)
        {
            StopAgent();
            Attack(hitEntities[0]);
        }
        //}
    }

    public void Attack(Collider2D unit)
    {
        if (_isDie) return;
        _nextAttackTime += Time.deltaTime;

        if (_nextAttackTime >= _attackSpeed)
        {
            StartAnimationAttack();
            TakeDamage(unit);
            PlaySoundAttack();
            if (_typeEnym != TypeEnym.TNT)
            {
                StopAgent();
            }
            else
            {
                StopAgent();
                _isDie = true;
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
            Warrior warrior = unit.GetComponent<Warrior>();
            if(warrior)
                warrior.TakeDamage(_attack);

            Archer archer = unit.GetComponent<Archer>();
            if (archer)
                archer.TakeDamage(_attack);

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

        _health -= DamageÑalculation(damage);
        HealthBarAmountFillAmount(_health);

        if (_health <= 0)
        {
           Die();
        }
    }

    public void InitEnemy(int hp, int def, int atk, int countStek)
    {
        _health = hp;
        _attack = atk;
        _defence = def;
        HealthBarAmountFillAmount(_health);
        Move(_tagetPosition);
    }

    public void InitEnemy()
    {
        Move(_tagetPosition);
    }

    private float DamageÑalculation(float damage)
    {
        if (damage > _defence)
            return (damage - _defence ) / _health;
        else
            return (_defence  - damage) / _health;
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
