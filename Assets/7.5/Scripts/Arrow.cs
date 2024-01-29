using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{
    public float speedArrow;
    private Transform _targetPosition;
    private bool _isTarget;
    private Rigidbody2D _rigidbody2D;
    private bool _isShoot;

    private void Update()
    {
        if (_isShoot)
            MoveArrow();
    }

    private void OnValidate()
    {
        GetRigidbody2D();
    }

    private void GetRigidbody2D() => _rigidbody2D ??= GetComponent<Rigidbody2D>();
    private void MoveArrow()
    {
        if(!_isTarget)
        {
            //_rigidbody2D.AddForce(_targetPosition.up * 20f);
        }
    }

    public void ShootArrow(Transform targetPosition)
    {
        _targetPosition = targetPosition;
        _isShoot = true;
    }
}
