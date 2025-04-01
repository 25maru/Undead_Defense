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
            Debug.Log($"{gameObject}�� �ڽĿ� healthBar ��ũ��Ʈ�� �پ������ʽ��ϴ�.");
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
        
        if(hp <= 0)
            OnDeath?.Invoke();
        
        return hp <= 0;
    }

    public void Heal(float heal)
    {
        hp = Mathf.Min(hp + heal, _maxHp);
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
