using System.Collections;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private float _speedArrow;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private LayerMask _layerMask;
    private BoxCollider2D _collider2d;
    private float _attack;
    private Random _random;
    private int _minPositionY = -9;
    private int _maxPositionY = -7;
    private float _offsetY = 0.5f;

    private void Start()
    {
        _collider2d = gameObject.GetComponent<BoxCollider2D>();
        _random = new Random();
        StartCoroutine(DestroyArrow());
    }

    private void Update()
    {
        StopArrow();
    }

    private void StopArrow()
    {
        if (transform.position.y <= _random.Next(_minPositionY, _maxPositionY) * _offsetY)
            _rigidbody2D.Sleep();
    }

    private void OnValidate()
    {
        GetRigidbody2D();
    }

    private void GetRigidbody2D() => _rigidbody2D ??= GetComponent<Rigidbody2D>();

    public void ShootArrow(float attack, Vector3 targetPosition)
    {
        _attack = attack;
        Vector3 shotDirection = targetPosition - transform.position;
        transform.right = shotDirection;
        _rigidbody2D.AddForce(shotDirection * _speedArrow * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer&(1<<_layerMask.value))==0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                _collider2d.enabled = false;
                enemy.TakeDamage(_attack);
                _rigidbody2D.Sleep();
                enabled = false;
            }
        }
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(_random.Next(1, 5)*0.4f);
        Destroy(gameObject);
    }
}
