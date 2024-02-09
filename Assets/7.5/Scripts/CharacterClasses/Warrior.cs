using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Warrior : Entity, IDamageable, IMovable, IAttack
{
    public static event Action Deathing;
    public static event Action<int> EatUp;

    [SerializeField] private float _baffAttack;
    [SerializeField] private float _baffDefence;
    [SerializeField] private float _baffHealth;
    [SerializeField] private SoundClip _needMeat;

    [SerializeField] private Image _eatBar;
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private int _eatUp;
    [SerializeField] private float _eatUpCycle;
    private float _currentTimeEat;
    private float _satiety = 100;

    private Vector3 _target;
    private bool _buffUse;
    private Buff _eatBuff;

    protected override void Awake()
    {
        base.Awake();
        _eatUpCycle = _playerData.warriorEatTimer;
        _eatUp = _playerData.warriorEatUpCycle;
        PlaySoundNewEntity();
        _healthFull = _health;
        //BaffSatiety();
        EatBarAmountFillAmount(_satiety);
        HealthBarAmountFillAmount(_health);
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(_attackPoint.position, _target);
    }

    private void Update()
    {
        if (_isDie) return;
        BaffSatiety();
        DetectEntity();
        DetectHitEntity();
        EatUpCycle();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer & (1 << _layerMask.value)) == 0)
        {
            _target = collision.gameObject.transform.position;
            MoveToEntity(collision.gameObject.transform.position);
        }else
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

    private Enemy FindEnemy() => FindObjectOfType<Enemy>();

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
            //BaffSatiety();
            if (Storage.Meat > 0)
            {
                EatUp?.Invoke(_eatUp);
            }
            else
            {
                Debug.Log("çàêîí÷èëàñü åäà, ïîêàçàòåëè âîèíîâ ñíèæåíû");
            }
            _currentTimeEat -= _eatUpCycle;
        }
    }
        

    private void BaffSatiety()
    {
        if(Storage.Meat > _eatUp)
        {
            _baffAttack = _attack* _eatBuff.Attack;
            _baffDefence = _defence * _eatBuff.Defence;
            _baffHealth = _health*_eatBuff.Health;

            if (!_buffUse)
            {
                _health += _baffHealth;
                _buffUse = true;
            }
        }
        else
        {
            _baffAttack = 0;
            _baffDefence = 0;
            _baffHealth = 0;

            if (_buffUse)
            {
                _health = _healthFull;
                _buffUse = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        BaffSatiety();
        SetTriggerAnimation("Hit");
        if (damage <= 0) return;
        _health -= DamageÑalculation(damage);
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
        Vector3 newPoint = new Vector3(newPosition.position.x + (_random.Next(-5, 6) + 0.1f) * 0.6f, newPosition.position.y + (_random.Next(-2, 3) + 0.1f) * 0.3f);
        _tagetPosition = newPoint;
        Move(_tagetPosition);
    }

    public void FindEnemyPosition(Vector3 castlePosition)
    {
        Move(castlePosition);
    }

    public void EatBuff(Buff buff)
    {
        _eatBuff = buff;
        BaffSatiety();
    }
}
