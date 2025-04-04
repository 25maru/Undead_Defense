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
        Rotate();
    }
    void Rotate()
    {
        float rotateDgree = Mathf.Atan2(player.curMoveInput.x, player.curMoveInput.y);
        rotateDgree *= Mathf.Rad2Deg;
        model.localRotation = Quaternion.RotateTowards(model.localRotation, Quaternion.Euler(0,rotateDgree,0),player.rotateSpeed);
    }
}
