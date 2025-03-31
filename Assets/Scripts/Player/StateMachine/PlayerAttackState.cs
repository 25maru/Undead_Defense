using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        if(player.target == null)
        {
            playerStateMachine.ChangeState(playerStateMachine.IdleState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        Rotate();
    }
    void Rotate()
    {
        Vector3 direction = player.target.position - player.transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            player.model.transform.localRotation = Quaternion.RotateTowards(model.transform.localRotation, targetRotation, player.rotateSpeed);
        }
    }
}
