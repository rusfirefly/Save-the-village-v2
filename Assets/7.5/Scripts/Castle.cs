using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public static event Action CastleAttaking;
    [SerializeField] private float _health;

    public void TakeDamage(float damage)
    {
        CastleAttaking?.Invoke();
        _health -= damage;
        Debug.Log(_health);
        if (_health <= 0)
            Debug.Log("Game Over!");
    }
}
