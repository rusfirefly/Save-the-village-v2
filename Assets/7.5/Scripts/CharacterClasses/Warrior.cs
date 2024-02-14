using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Warrior : Entity, IDamageable, IMovable, IAttack, IBuffable
{
    public static event Action Deathing;
    public static event Action<int> EatUp;

    [SerializeField] private float _baffAttack;
    [SerializeField] private float _baffDefence;
    [SerializeField] private SoundClip _needMeat;

    [SerializeField] private Image _eatBar;
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private int _eatUp;
    [SerializeField] private float _eatUpCycle;
    private float _currentTimeEat;

    private Vector3 _target;
    private Buff _eatBuff;
    
    private const int _xRandomMin = -5;
    private const int _xRandomMax = 6;
    private const float _xKoef = 0.1f;
    private const float _xOffset = 0.6f;

    private const int _yRandomMin = -2;
    private const int _yRandomMax = 3;
    private const float _yKoef = 0.1f;
    private const float _yOffset = 0.3f;

    private Random _randomPosition;

    private float _currentHealth;

    protected override void Awake()
    {
        base.Awake();
        SetStartValueEat();
        PlaySoundNewEntity();
        _healthFull = _health;
        _currentHealth = _health;
        EatBarAmountFillAmount(_eatUpCycle);
        HealthBarAmountFillAmount(_health);
        _randomPosition = new Random();
        BuffSkill.EventBuff += OnEventBuff;
        Castle.Attacked += OnCastleAttakedEvent;
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(_attackPoint.position, _target);
    }

    private void Update()
    {
        if (_isDie) return;
        DetectEntity();
        DetectHitEntity();
        EatUpCycle();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer & (1 << _layerMask.value)) == 0)
        {
            _target = collision.gameObject.transform.position;
            MoveToEntity(collision.gameObject.transform.position);
        }
        else
        {
            GoBackPosition();
        }
    }

    private void OnDestroy()
    {
        BuffSkill.EventBuff -= OnEventBuff;
        Castle.Attacked -= OnCastleAttakedEvent;
    }

    private void OnCastleAttakedEvent(Vector3 castlePosition)
    {
        FindEnemyPosition(castlePosition);
    }

    private void OnEventBuff(Buff buff)
    {
        AddBuff(buff);
    }

    public void AddBuff(Buff buff)
    {
        _eatBuff = buff;
        UpdateBuff();
    }

    private void SetStartValueEat()
    {
        _eatUpCycle = _playerData.warriorEatTimer;
        _eatUp = _playerData.warriorEatUpCycle;
    }

    private void DetectEntity()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _distanceFindEntity, _layerMask);
        foreach(Collider2D collider in colliders)
        {
            MoveToEntity(collider.transform.position);
        }

        if(colliders.Length==0)
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

    private void DetectHitEntity()
    {
        Collider2D[] hitEntities = HitEntity();
        foreach (Collider2D entity in hitEntities)
        {
            StopAgent();
            Attack(entity);
        }
    }

    private void MoveToEntity(Vector3 enemy)
    {
        float dist = GetDistanceToEntity(enemy);
        if (CheckDistanceToEnemy(dist, _distanceFindEntity))
        {
            _target = enemy;
            GoToEntity(enemy);
        }
    }

    private bool CheckDistanceToEnemy(float currentDistance, float distance)
    {
        if (currentDistance <= distance) return true;
        else return false;
    }
   
    private void GoToEntity(Vector3 enemyPosition)
    {
        _speedEntity = _stepEntity * Time.deltaTime;
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, enemyPosition, _speedEntity);
    }

    private float GetDistanceToEntity(Vector3 enemyPosition)
    {
        return Vector2.Distance(gameObject.transform.position, enemyPosition);
    }

    public void Attack(Collider2D unit)
    {
        _nextAttackTime += Time.deltaTime;
        if (_nextAttackTime >= _attackSpeed)
        {
            SetTriggerAnimation("Attack1");
            unit.GetComponent<Enemy>().TakeDamage(_attack + _baffAttack);
            PlaySoundAttack();
            _nextAttackTime = 0;
        }
    }

    private void EatUpCycle()
    {
        _currentTimeEat += Time.deltaTime;
        EatBarAmountFillAmount(_eatUpCycle - _currentTimeEat);
        if (_currentTimeEat >= _eatUpCycle)
        {
            if (Storage.Meat > 0)
            {
                EatUp?.Invoke(_eatUp);
            }
            
            _currentTimeEat -= _eatUpCycle;
        }
    }
        
    public void UpdateBuff()
    {
        _baffAttack = _attack * _eatBuff.Attack;
        _baffDefence = _defence * _eatBuff.Defence;
        _health = _currentHealth + _currentHealth * _eatBuff.Health;
    }

    public void TakeDamage(float damage)
    {
        SetTriggerAnimation("Hit");
        if (damage <= 0) return;
        _health -= DamageÑalculation(damage);
        _currentHealth = _health;
        HealthBarAmountFillAmount(_health, _eatBuff.Health);
        if (_health <= 0)
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
        if (damage > (_defence + _baffDefence))
            return (damage - (_defence + _baffDefence));
        else
            return ((_defence + _baffDefence) - damage);
    }

    private void EatBarAmountFillAmount(float value) => _eatBar.fillAmount = value / _eatUpCycle;

    public void GoToNewTargetPosition(Transform newPosition)
    {
        Vector3 newPoint = SetRandomPosition(newPosition.position);
        _tagetPosition = newPoint;
        Move(_tagetPosition);
    }

    private Vector3 SetRandomPosition(Vector3 position)
    {
        return new Vector3(position.x + (_randomPosition.Next(_xRandomMin, _xRandomMax) + _xKoef) * _xOffset, position.y + (_randomPosition.Next(_yRandomMin, _yRandomMax) +_yKoef) * _yOffset);
    }

    public void FindEnemyPosition(Vector3 castlePosition)
    {
        Move(castlePosition);
    }
}
