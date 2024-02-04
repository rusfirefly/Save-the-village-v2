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

   
    private float _satiety = 100;
    private float _currentTimeEat;
    [SerializeField] private int _eatUp = 15;
    [SerializeField] private float _eatUpCycle=10;

    protected override void Awake()
    {
        base.Awake();
        PlaySoundNewEntity();
       // _eatUpCycle = PlayerData.
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
        if(_currentTimeEat >= _eatUpCycle)
        {
            if (PlayerBase.meat >= 0)
                EatUp?.Invoke(_eatUp);
            else
                Debug.Log("çàêîí÷èëàñü åäà, ïîêàçàòåëè âîèíîâ ñíèæåíû");

            _currentTimeEat -= _eatUpCycle;
        }
    }
        

    private void BaffSatiety()
    {
        if(PlayerBase.meat>=_eatUpCycle)
        {
            _baffAttack = 0;
            _baffDefence = 0;
            _baffHealth = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        BaffSatiety();
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
        if (damage > (_defence+_baffDefence) * _countInStek)
            return (damage - ((_defence + _baffDefence) * _countInStek)) / _health;
        else
            return (((_defence + _baffDefence) * _countInStek) - damage) / _health;
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
}
