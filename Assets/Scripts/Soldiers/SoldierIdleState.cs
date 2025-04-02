using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierIdleState : SoldierBaseState
{

    public SoldierIdleState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(soldier.AnimationData.IdleParameterHash);
        soldier.agent.ResetPath();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(soldier.AnimationData.IdleParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if(soldier.target != null || soldier.orderTarget != null)
        {
            soldierStateMachine.ChangeState(soldierStateMachine.MoveState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move();
    }

    protected override void Move()
    {
        base.Move();
        if (!soldier.isGround)
        {
            soldier.Controller.Move(Vector3.down * soldier.downSpeed * Time.deltaTime);
        }
        
    }
}
