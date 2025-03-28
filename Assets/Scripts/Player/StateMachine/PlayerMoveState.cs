using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(playerStateMachine.Player.AnimationData.MoveParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(playerStateMachine.Player.AnimationData.MoveParameterHash);
    }
    public override void Update()
    {
        base.Update();

    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
