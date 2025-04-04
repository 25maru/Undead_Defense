using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : IState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected Transform model;


    public PlayerBaseState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        player = playerStateMachine.Player;
        model = player.model;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void Update()
    {
    }
    protected void StartAnimation(int animationHash)
    {
        player.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        player.animator.SetBool(animationHash, false);
    }
}
