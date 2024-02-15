using System;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Archer : Entity, IDamageable, IAttack, IMovable, IBuffable
{
    private SpriteRenderer _archer;
    public static event Action Deathing;
    public static event Action<int> EatUp;

    [SerializeField] private GameObject _arrowPrefab;
    [SerializeField] private SoundClip _needMeat;
    [SerializeField] private Image _eatBar;
    [SerializeField] private PlayerData _playerData;
    [SerializeField] private float _baffDefence;
    [SerializeField] private float _baffAttack;
    [SerializeField] private int _eatUp;
    [SerializeField] private float _eatUpCycle;

    private float _currentTimeEat;
    private Vector3 _target;
    private Collider2D colider;
    private Buff _eatBuff;
    private bool _archerInCastle;

    private const int _xRandomMin = -1;
    private const int _xRandomMax = 1;
    private const float _xKoef = 0.2f;
    private const float _xOffset = 0.5f;
    private Random _randomPosition;

    private float _currentHealth;

    protected override void Awake()
    {
        base.Awake();
        SetStartValueEat();
        PlaySoundNewEntity();
        _healthFull = _health;
        _currentHealth = _health;
        EatBarAmountFillAmount(_healthFull);
        HealthBarAmountFillAmount(_health);
        GetSpriteRenderArcher();
        _randomPosition = new Random();

        BuffSkill.EventBuff += OnEventBuff;
        GameManager.ReloadAll += OnReloadAll;
    }

    private void Update()
    {
        if (_isDie) return;
        DetectEnemy();
        EatUpCycle();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(_attackPoint.position, _target);
    }

    private void OnDestroy()
    {
        BuffSkill.EventBuff -= OnEventBuff;
        GameManager.ReloadAll -= OnReloadAll;
    }
    public void AddBuff(Buff buff)
    {
        _eatBuff = buff;
        UpdateBuff();
    }

    private void OnEventBuff(Buff buff)
    {
        AddBuff(buff);
    }
    
    private void OnReloadAll()
    {
        Destroy(gameObject);
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
                break;
            }
           index++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_archerInCastle) return;
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
        _tagetPosition = newPoint;
        Move(_tagetPosition);
    }

    private Vector3 GetOffsetRandomPosition(Vector3 position)
    {
        return new Vector3(position.x + (_randomPosition.Next(_xRandomMin, _xRandomMax) + _xKoef) * _xOffset, position.y);
    }

    public void FindEnemyPosition()
    {
        Enemy[] enemys = GameObject.FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemys)
        {
            _agent.destination = enemy.gameObject.transform.position;
        }
    }
}
