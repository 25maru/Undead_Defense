using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _maxHp = 100;
    [SerializeField] private float _hp;
    public Action OnDeath;  // 사망 이벤트
    
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

    public bool OnDamaged(float damage)
    {
        hp -= damage;
        
        if(hp <= 0)
            OnDeath?.Invoke();
        
        return hp <= 0;
    }

    public void Heal(float heal)
    {
        hp = Mathf.Min(hp + heal, _maxHp);
    }
}
