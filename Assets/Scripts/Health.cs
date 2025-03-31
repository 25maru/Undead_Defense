using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _maxHp = 100;
    [SerializeField] private float _hp;
    
    public float maxHp
    {
        get => _maxHp;
        set => _maxHp = value;
    }

    public float hp { get => _hp;
        set => _hp = value; }

    private void OnEnable()
    {
        _hp = _maxHp;
    }

    public void OnDamaged(float damage)
    {
        hp -= damage;
    }
}
