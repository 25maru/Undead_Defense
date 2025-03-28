using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMoveState : SoldierBaseState
{
    public SoldierMoveState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
    {
    }
    public override void Enter()
    {

        base.Enter();
        StartAnimation(soldier.AnimationData.MoveParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(soldier.AnimationData.MoveParameterHash);
    }
    public override void Update()
    {
        base.Update();
        CheckOrderPosition();
        if(soldier.target == null)
        {
            soldierStateMachine.ChangeState(soldierStateMachine.IdleState);
            return;
        }
        if(Vector3.Distance(soldier.target.position,soldier.transform.position) <= soldier.attackDistance /* - 0.1f ¿©À¯°ª */ )
        {
            soldierStateMachine.ChangeState(soldierStateMachine.AttackState);
            return;
        }
    }
    public override void HandleInput()
    {
        base.HandleInput();
        soldier.agent.SetDestination(soldier.orderTarget != null ? soldier.orderTarget.position : soldier.target.position);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move();
    }
    void Move()
    {
        
    }
    void CheckOrderPosition()
    {
        if(soldier.orderTarget != null && Vector3.Distance(soldier.orderTarget.position,soldier.transform.position) <= 0.1f)
        {
            soldier.ClearOrderTarget();
        }
    }
}
