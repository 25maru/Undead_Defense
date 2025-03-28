using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public Animator animator;
    public Vector3 moveDirection;
    public Transform target;

    public float rotateSpeed;
    public CharacterController Controller { get; private set; }
    [field: SerializeField] public AnimationData AnimationData { get; private set; }

    private void Awake()
    {
        AnimationData.Initialize();
        Controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        playerStateMachine = new PlayerStateMachine(this);

    }
    private void Update()
    {
        playerStateMachine.HandleInput();
        playerStateMachine.Update();
    }
    private void FixedUpdate()
    {
        playerStateMachine.PhysicsUpdate();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            playerStateMachine.ChangeState(playerStateMachine.MoveState);
        }
    }
}
