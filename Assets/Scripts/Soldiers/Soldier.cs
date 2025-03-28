using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    public SoldierStateMachine soldierStateMachine;
    public Animator animator;
    public Transform model;
    public List<Transform> targets;
    public Transform target;

    public float rotateSpeed;
    public float moveSpeed;

    public CharacterController Controller { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();

        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        soldierStateMachine = new SoldierStateMachine(this);

    }
    private void Update()
    {
        soldierStateMachine.HandleInput();
        soldierStateMachine.Update();
    }
    private void FixedUpdate()
    {
        soldierStateMachine.PhysicsUpdate();
       
    }
}
