using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Archer : Entity, IDamageable, IAttack, IMovable
{
    private SpriteRenderer _archer;
    public static event Action Deathing;
    public static event Action<int> EatUp;

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private SoundClip _needMeat;
    [SerializeField] private Image _eatBar;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private float _baffHealth;
    [SerializeField] private float _baffDefence;
    [SerializeField] private float _baffAttack;
    [SerializeField] private int _eatUp;
    [SerializeField] private float _eatUpCycle;

    private float _currentTimeEat;
    private Vector3 _target;
    private bool _buffUse;
    private Collider2D colider;
    private Buff _eatBuff;


    private const int _xRandomMin = 1;
    private const int _xRandomMax = 2;
    private const float _xKoef = 0.1f;
    private const float _xOffset = 0.6f;



    protected override void Awake()
    {
        base.Awake();
        SetStartValueEat();
        PlaySoundNewEntity();
        BaffSatiety();
        _healthFull = _health + _baffHealth;
        EatBarAmountFillAmount(_healthFull);
        HealthBarAmountFillAmount(_health);
        GetSpriteRenderArcher();
    }

    private void Update()
    {
        if (_isDie) return;
        DetectEnemy();
        //Attack(colider);
        EatUpCycle();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(_attackPoint.position, _target);
    }

    private void GetSpriteRenderArcher()
    {
        _archer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void SetStartValueEat()
    {
        _eatUpCycle = _playerData.archerEatTimer;
        _eatUp = _playerData.archerEatUpCycle;
    }

    protected void DetectEnemy()
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, _attackRange, _layerMask);
        int index = 0;
        foreach(RaycastHit2D hit in hits)
        {
           float dist = GetDistanceToEntity(hit.transform.position);
            if (dist <= _attackRange)
            {
                _target = hits[index].collider.gameObject.transform.position;
                colider = hits[index].collider;
                Attack(colider);
                //ArrowShoot(colider.gameObject.transform.position);
                break;
            }
           index++;
        }

        if (hits.Length > 0 && colider == null)
        {
            //_target = hits[index].collider.gameObject.transform.position;
            //colider = hits[index].collider;
            //ArrowShoot(colider.gameObject.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Castle")
        {
            collision.GetComponent<Castle>().ArcherOnCastle(this);  
        }
    }
    private float GetDistanceToEntity(Vector3 enemyPosition)
    {
        return Vector2.Distance(gameObject.transform.position, enemyPosition);
    }

    private void FlipArcher(float x)
    {
        if (x < transform.position.x)
            _archer.flipX = true;
        else
            _archer.flipX = false;
    }

    protected override void Die()
    {
        base.Die();

        Deathing?.Invoke();
        DisableScript();
    }

    public void Attack(Collider2D unit)
    {
        if (unit == null) return;
        _nextAttackTime += Time.deltaTime;
        if (_nextAttackTime >= _attackSpeed)
        {
            BaffSatiety();
            SetTriggerAnimation("Attack2");
            ArrowShoot(unit.gameObject.transform.position);
            PlaySoundAttack();
            colider = null;
            _nextAttackTime = 0;
        }
    }

    private void ArrowShoot(Vector3 position)
    {
        Arrow arrow = Instantiate(_arrowPrefab, _attackPoint.position, _attackPoint.rotation).GetComponent<Arrow>();
        arrow.ShootArrow(_attack + _baffAttack, position);
    }

    private void EatUpCycle()
    {
        _currentTimeEat += Time.deltaTime;
        EatBarAmountFillAmount(_eatUpCycle - _currentTimeEat);
        if (_currentTimeEat >= _eatUpCycle)
        {
            BaffSatiety();
            if (Storage.Meat > 0)
            {
                EatUp?.Invoke(_eatUp);
            }
            else
                Debug.Log("çàêîí÷èëàñü åäà, ïîêàçàòåëè âîèíîâ ñíèæåíû");

            _currentTimeEat -= _eatUpCycle;
        }
    }


    private void BaffSatiety()
    {
        if (Storage.Meat > _eatUp)
        {
            _baffAttack = _attack * _eatBuff.Attack;
            _baffDefence = _defence * _eatBuff.Defence;
            _baffHealth = _health * _eatBuff.Health;


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
        HealthBarAmountFillAmount(_health);
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
        Vector3 newPoint = GetOffsetRandomPosition(newPosition.position);
        // _tagetPosition = newPoint;
        Move(newPoint);
    }
    private Vector3 GetOffsetRandomPosition(Vector3 position)
    {
        return new Vector3(position.x + (_random.Next(_xRandomMin, _xRandomMax) + _xKoef) * _xOffset, position.y);
    }

    public void FindEnemyPosition()
    {
        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
        {
            _agent.destination = enemy.gameObject.transform.position;
        }
    }

    public void EatBuff(Buff buff)
    {
        _eatBuff = buff;
    }
}
