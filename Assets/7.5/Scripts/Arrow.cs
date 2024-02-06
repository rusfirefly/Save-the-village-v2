using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    public float speedArrow;
    private Vector2 _targetPosition;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private LayerMask _layerMask;
    private BoxCollider2D _collider2d;
    private float _attack;


    private void Start()
    {
        _collider2d = gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(DestroyArrow());
    }

    private void Update()
    {
     
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
        _rigidbody2D.AddForce(shotDirection * speedArrow, ForceMode2D.Impulse);
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
                _rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }


    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

}
