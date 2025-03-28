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
}
