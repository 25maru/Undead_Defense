using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] protected float _maxHp = 100;
    [SerializeField] private float _hp;
    event Action onDeath;
    HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
        if(healthBar == null)
        {
            Debug.Log($"{gameObject}의 자식에 healthBar 스크립트가 붙어있지않습니다.");
        }
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }
    public float maxHp
    {
        get => _maxHp;
        set => _maxHp = value;
    }

    public float hp
    {
        get { return _hp; }
        set { ChangeHP(value); }
    }

    private void OnEnable()
    {
        hp = maxHp;
    }

    public bool OnDamaged(float damage)
    {
        hp -= damage;
        
        return hp <= 0;
    }

    public void ChangeHP(float value)
    {
        _hp = Mathf.Clamp(value, 0, maxHp);

        healthBar.SetHealth(hp / maxHp);

        if (_hp == 0)
        {
            onDeath?.Invoke();
        }
        
    }
    public void AddDieEvent(Action dieEvent)
    {
        onDeath += dieEvent;
    }
}
