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

    private float _satiety = 100;
    private float _currentTimeEat;
    [SerializeField] private int _eatUp;
    [SerializeField] private float _eatUpCycle;

    protected override void Awake()
    {
        base.Awake();
        _eatUpCycle = _playerData.warriorEatTimer;
        _eatUp = _playerData.warriorEatUpCycle;
        PlaySoundNewEntity();
        _healthFull = _health + _baffHealth;
        BaffSatiety();
        EatBarAmountFillAmount(_satiety);
        HealthBarAmountFillAmount(_health);
    }

    private void Update()
    {
        if (_isDie) return;
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
        EatUpCycle();
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
            BaffSatiety();
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
            BaffSatiety();
            if (Storage.Meat >= 0)
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
        if(Storage.Meat>=_eatUpCycle)
        {
            _baffAttack = _attack*0.15f;
            _baffDefence = _defence * 0.15f;
            _baffHealth = _health*0.5f;
        }else
        {
            _baffAttack = _attack * 0.15f * -1;
            _baffDefence = _defence * 0.15f * -1;
            _baffHealth = _health * 0.5f * -1;
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
}
