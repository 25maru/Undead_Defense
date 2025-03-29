using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public Animator animator;
    public Vector2 curMoveInput;
    public Transform model;
    public List<Transform> targets;
    public Transform target;

    public float rotateSpeed;
    [SerializeField] float moveSpeed;
    public CharacterController Controller { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();
        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerStateMachine = new PlayerStateMachine(this);

    }
    private void Update()
    {
        SetTarget();
        playerStateMachine.HandleInput();
        playerStateMachine.Update();
    }
    private void FixedUpdate()
    {
        playerStateMachine.PhysicsUpdate();
        move();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMoveInput = context.ReadValue<Vector2>();
            playerStateMachine.ChangeState(playerStateMachine.MoveState);
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
            playerStateMachine.ChangeState(playerStateMachine.IdleState);
        }
    }
    void move()
    {
        Vector3 movedirection = new Vector3(curMoveInput.x, 0 ,curMoveInput.y);
        Controller.Move(movedirection * moveSpeed * Time.deltaTime);
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     targets.Add(other.transform);
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //     targets.Remove(other.transform);
    // }
    void SetTarget()
    {
        if(targets.Count == 0)
        {
            target = null;
            return;
        }
        for (int i = 0; i < targets.Count; i++)
        {
            if (target == null)
                target = targets[i];
            if((target.position -transform.position).sqrMagnitude > (targets[i].position - transform.position).sqrMagnitude)
                target = targets[i];
        }
    }
}
