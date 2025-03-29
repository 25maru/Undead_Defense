using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    


    [SerializeField] SphereCollider detectingCollider;
    [SerializeField] GameObject underOrderCircle;

    public float rotateSpeed;
    public float moveSpeed;

    public CharacterController Controller { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();

        agent = GetComponent<NavMeshAgent>();
        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        soldierStateMachine = new SoldierStateMachine(this);

    }
    private void Start()
    {
        soldierStateMachine.ChangeState(soldierStateMachine.IdleState);
        detectingCollider.radius = detectingDistance;                     //유닛 데이터에서 각 유닛별 감지거리만큼 콜라이더 변형
    }
    private void Update()
    {
        SetTarget();
        soldierStateMachine.HandleInput();
        soldierStateMachine.Update();
    }
    private void FixedUpdate()
    {
        soldierStateMachine.PhysicsUpdate();
       
    }
    private void OnTriggerEnter(Collider other)
    {
        targets.Add(other.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        targets.Remove(other.transform);
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
    public void SetOrderTarget(Transform transform)
    {
        orderTarget = transform;
        underOrderCircle.SetActive(true);
    }
    public void ClearOrderTarget()
    {
        orderTarget = null;
        underOrderCircle.SetActive(false);
    }
}
