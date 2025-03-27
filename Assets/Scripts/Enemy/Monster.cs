using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [Header("Monster Status")]
    public string monsterName;
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected float maxHp = 100;
    [SerializeField] private float _hp;
    [SerializeField] protected float _speed;
    public float hp { get => _hp; private set => _hp = value; }
    public float speed { get => _speed; private set => _speed = value; }

    [Header("Monster Attack")]
    [SerializeField] private int attackPower;
    public float distance;      // 공격 가능 거리
    public bool inRange;
    
    [Header("HP Bar")]
    [SerializeField] protected RectTransform hpBar;
    
    [Header("Monster AI")]
    [SerializeField] private Transform target;
    
    protected NavMeshAgent navMeshAgent;
    protected Animator anim;
    protected State state = State.Idle;
    
    private static readonly int idleState = Animator.StringToHash("Base Layer.Idle");
    private static readonly int moveState = Animator.StringToHash("Base Layer.Move");
    private static readonly int damagedState = Animator.StringToHash("Base Layer.Damaged");
    private static readonly int attackState = Animator.StringToHash("Base Layer.Attack");
    private static readonly int dieState = Animator.StringToHash("Base Layer.Die");
    
    

    public Action action;
    protected enum State
    {
        Idle,
        Move,
        Attack,
        Dead
    }// 몬스터 상태
    
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        // target = null;  // 나중에 메인 기지 세팅
        action += OnDead;
    }
    
    private void OnEnable()
    {
        isDead = false;
        _hp = maxHp;
        navMeshAgent.speed = _speed;
        navMeshAgent.SetDestination(target.position);
    }

    protected virtual void Update()
    {
        if(isDead) return;
        
        anim.SetBool("Move" , state != State.Idle);
        anim.SetBool("Attack", state == State.Attack);
        
        
        if(inRange)
            Attack();
        
        Move();
    }

    protected virtual void Attack()
    {
        ChangeState(State.Attack);
    }

    protected virtual void Move()
    {
        if (Vector3.Distance(transform.position, target.position) <= distance)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
            inRange = true;
            ChangeState(State.Attack);
        }
        else
        {
            inRange = false;
            SetNav();
            ChangeState(State.Move);
        }
    }

    protected virtual void SetNav()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);
    }

    protected virtual void OnDead()
    {
        anim.CrossFade(dieState, 0.1f);
        DropItem();
    }

    protected virtual void DropItem()
    {
        
    }
    
    protected void ChangeState(State newState)
    {
        state = newState;
    }
    
    public void OnHit(float dmg) // 피격시 호출되는 메서드.
    {
        hp -= dmg;
    }
    
    protected virtual float GetHpPercent()
    {
        return hp / maxHp;
    }
}
