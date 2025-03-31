using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHoldState : SoldierBaseState
{
    public SoldierHoldState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(soldier.AnimationData.IdleParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(soldier.AnimationData.IdleParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if(soldier.target != null && Vector3.Distance(soldier.target.position, soldier.transform.position) <= soldier.attackDistance)
        {
            soldierStateMachine.ChangeState(soldierStateMachine.AttackState);
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
