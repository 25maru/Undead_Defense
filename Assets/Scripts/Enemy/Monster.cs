using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IChase
{
    float chaseDistance { get; set; }
    void ChaseTarget();
}

public class Monster : MonoBehaviour, IChase
{
    [Header("Monster Status")]
    public string monsterName;
    public Health health;
    [SerializeField] protected bool isDead = false;
    [SerializeField] protected float _speed;
    public float speed { get => _speed; private set => _speed = value; }

    [Header("Monster Attack")]
    [SerializeField] private int attackPower;
    public float attackRate;
    private float lastAttackTime;
    public SphereCollider attackRangeCollider;
    public float distance;      // 공격 가능 거리
    public bool inRange;

    [Header("Monster Chase")] 
    public float chaseRate;     // 몬스터 검색 범위 벗어날 경우 n초간 따라감
    [field: SerializeField] public float chaseDistance { get; set; } // 타겟 검색 거리
    private float lastChaseTime;
    private bool isChase;
    
    [Header("HP Bar")]
    [SerializeField] protected HealthBar hpBar;
    
    [Header("Monster AI")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask targetLayer;
    
    protected NavMeshAgent navMeshAgent;
    protected Animator anim;
    protected State state = State.Idle;
    
    private static readonly int moveState = Animator.StringToHash("Move");
    private static readonly int damagedState = Animator.StringToHash("Damaged");
    private static readonly int attackState = Animator.StringToHash("Attack");
    private static readonly int dieState = Animator.StringToHash("Base Layer.Die");
    
    public Material[] materials;
    
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
        health = GetComponent<Health>();
        anim = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        attackRangeCollider = GetComponentInChildren<SphereCollider>();
        hpBar = GetComponent<HealthBar>();
        target = TestHomeTarget.Instance.transform;  // 나중에 메인 기지 세팅
        action += OnDead;
        
        SearchMaterial();
    }
    
    private void OnEnable()
    {
        isDead = false;
        isChase = true;
        attackRangeCollider.radius = distance;
        navMeshAgent.speed = _speed;
        navMeshAgent.SetDestination(target.position);
    }

    protected virtual void Update()
    {
        if(isDead) return;
        
        anim.SetBool(moveState , state != State.Idle);
        anim.SetBool(attackState, state == State.Attack);
        
        if(inRange)
            Attack();
        
        Move();
    }

    protected virtual void Attack()
    {
        if (target == null)
        {
            inRange = false;
            target = TestHomeTarget.Instance.transform;
        }
        
        // 공격시 목표물로 방향회전
        Vector3 direction = target.position - transform.position; direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        
        if(Time.time - lastAttackTime > attackRate)
        {
            Debug.Log("Attack");
            lastAttackTime = Time.time;
            StartCoroutine(DelayAttack(0.6f));
            
            ChangeState(State.Attack);
        }
    }

    IEnumerator DelayAttack(float time)
    {
        yield return new WaitForSeconds(time);

        if (target.gameObject.layer == LayerMask.NameToLayer("Player") ||
            target.gameObject.layer == LayerMask.NameToLayer("Soldier"))
        {
           var targetHealth = target.GetComponent<Health>();
           targetHealth?.OnDamaged(attackPower);
        }

        if (target.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            var targetBuilding = target.GetComponent<BuildingLogicController>();
            targetBuilding.TakeDamage(attackPower);
        }
    }
    

    protected virtual void Move()
    {
        if (inRange)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.velocity = Vector3.zero;
        }
        else
        {
            Vector3 velocity = Vector3.zero;
            transform.position =
                Vector3.SmoothDamp(transform.position, navMeshAgent.nextPosition,
                    ref velocity, 0.1f);
            
            SetNav();
            ChangeState(State.Move);
        }
        
        ChaseTarget();
    }

    protected virtual void SetNav()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(target.position);
    }

    public void ChaseTarget()
    {
        Collider[] colliders;
        if (isChase)
        {
            colliders = Physics.OverlapSphere(gameObject.transform.position, chaseDistance, targetLayer);

            if (colliders.Length > 0)
            {
                target = colliders[0].transform;
                isChase = false;
            }
        }
        else
        {
            if (Time.time - lastChaseTime > chaseRate)
            {
                lastChaseTime = Time.time;
                
                // 나중에 본거지 설정핳것!
                target = TestHomeTarget.Instance.gameObject.transform;
                isChase = true;
            }
        }
        
    }
    
    //피격시 호출되는 메서드
    public void OnHit(float damage)
    {
        if (health.OnDamaged(damage))
            action?.Invoke();
        
        hpBar?.SetHealth(health.hp / health.maxHp);
        StartCoroutine("DamageFlash");
    }
    
    IEnumerator DamageFlash()
    {
        Color[] color = new Color[materials.Length];

        for (int i = 0; i < materials.Length; i++)
        {
            color[i] = materials[i].color;
            materials[i].color = Color.white;
        }

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < materials.Length; i++)
            materials[i].color = color[i];
    }
    
    protected virtual void OnDead()
    {
        anim.CrossFade(dieState, 0.1f);
        isDead = true;
        ChangeState(State.Dead);
        
        DropItem();
        
        Destroy(gameObject, 1f);
    }

    protected virtual void DropItem()
    {
        
    }
    
    protected void ChangeState(State newState)
    {
        state = newState;
    }

    private void SearchMaterial()
    {
        // 모든 자식 오브젝트의 Renderer를 수집
        List<Material> materialList = new List<Material>();
        // 부모와 자식을 포함하여 모든 Renderer 탐색
        MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] skinedRenderers = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (Renderer renderer in renderers)
        {
            materialList.AddRange(renderer.materials);
        }
        foreach (Renderer renderer in skinedRenderers)
        {
            materialList.AddRange(renderer.materials);
        }
        materials = materialList.ToArray();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, distance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            inRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            inRange = true;
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            inRange = false;
        }
    }
}
