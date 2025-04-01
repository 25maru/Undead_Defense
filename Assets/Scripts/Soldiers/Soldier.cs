using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Soldier : MonoBehaviour
{


    public SoldierStateMachine soldierStateMachine;
    public Animator animator;
    public Transform model;
    public List<Transform> targets;
    public Transform target;
    public Transform orderTarget;
    public NavMeshAgent agent;

    public float detectingDistance;
    public float attackDistance;

    public bool isGround;
    [SerializeField] LayerMask groundMask;
    
    public Health health;

    [SerializeField] SphereCollider detectingCollider;
    [SerializeField] GameObject underOrderCircle;

    public float rotateSpeed;
    public float moveSpeed;
    public float downSpeed;
    public float damage;
    public float projectileSpeed;

    public CharacterController Controller { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();

        health = GetComponent<Health>();
        health.AddDieEvent(OnDeath);
        agent.updatePosition = false;
        agent.updateRotation = false;
        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        soldierStateMachine = new SoldierStateMachine(this);
    }
    private void Start()
    {
        soldierStateMachine.ChangeState(soldierStateMachine.IdleState);
        detectingCollider.radius = detectingDistance;                     //���� �����Ϳ��� �� ���ֺ� �����Ÿ���ŭ �ݶ��̴� ����
    }
    private void Update()
    {
        IsGround();
        SetTarget();
        soldierStateMachine.HandleInput();
        soldierStateMachine.Update();
    }
    private void FixedUpdate()
    {
        soldierStateMachine.PhysicsUpdate();
       
    }
    void SetTarget()
    {
        if (targets.Count == 0)
        {
            target = null;
            return;
        }
        for (int i = 0; i < targets.Count; i++)
        {
            if (target == null)
                target = targets[i];
            if ((target.position - transform.position).sqrMagnitude > (targets[i].position - transform.position).sqrMagnitude)
                target = targets[i];
        }
    }
    public void GetOrder(Transform transform)
    {
        orderTarget = transform;
        underOrderCircle.SetActive(true);
        soldierStateMachine.ChangeState(soldierStateMachine.FollowState);
    }
    public void MoveOrder()
    {
        soldierStateMachine.ChangeState(soldierStateMachine.MoveState);
    }
    public void EndOrder()
    {
        underOrderCircle.SetActive(false);
    }
    public void Hold()
    {
        orderTarget = null;
        soldierStateMachine.ChangeState(soldierStateMachine.HoldState);
    }
    void IsGround()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * 0.05f), Vector3.down, 0.1f, groundMask))
        {
            isGround = true;
        }
        else
        {
            isGround= false;
        }
    }

    void OnDeath()
    {
        soldierStateMachine.ChangeState(soldierStateMachine.DieState);
        soldierStateMachine.isDeath = true;
        Invoke("DeathDisable", 3f);
    }
    void DeathDisable()
    {
        gameObject.SetActive(false);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + (Vector3.up * 0.05f), Vector3.down * 0.1f);
    }
}
