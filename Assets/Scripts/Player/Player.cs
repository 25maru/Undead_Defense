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

    
    
    [SerializeField] PlayerOrderCollider playerOrderCollider;

    public float rotateSpeed;
    [SerializeField] float moveSpeed;
    public float damage;
    public float projectileSpeed;
    [SerializeField] float downSpeed;
    [SerializeField] LayerMask groundMask;
    public float detectingDistance;

    bool isGround;
    public Health health;
    bool isDeath = false;
    public CharacterController Controller { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        AnimationData = new AnimationData();
        AnimationData.Initialize();
        health = GetComponent<Health>();
        health.AddDieEvent(OnDeath);
        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerStateMachine = new PlayerStateMachine(this);

    }
    private void Update()
    {
        IsGround();
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
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMoveInput = Vector2.zero;
            playerStateMachine.ChangeState(playerStateMachine.IdleState);
        }
    }
    public void OnOrder(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerOrderCollider.StartOrder();
        }
        else if (context.canceled)
        {
            playerOrderCollider.EndOrder();
        }
    }
    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerOrderCollider.Hold();
        }
    }
    
    void move()
    {
        if(isDeath)
            return;
        Vector3 movedirection = new Vector3(curMoveInput.x, 0 ,curMoveInput.y);
        if (!isGround)
        {
            movedirection += Vector3.down * downSpeed;
        }
        Controller.Move(movedirection * moveSpeed * Time.deltaTime);
    }

    void SetTarget()
    {
        if(targets.Count == 0)
        {
            target = null;
            return;
        }
        targets.RemoveAll(item => item == null);
        for (int i = 0; i < targets.Count; i++)
        {
            if (target == null)
                target = targets[i];
            if((target.position -transform.position).sqrMagnitude > (targets[i].position - transform.position).sqrMagnitude)
                target = targets[i];
        }
    }
    void IsGround()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * 0.05f), Vector3.down, 0.1f, groundMask))
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    void OnDeath()
    {
        playerStateMachine.ChangeState(playerStateMachine.DieState);
        isDeath = true;
    }
}
