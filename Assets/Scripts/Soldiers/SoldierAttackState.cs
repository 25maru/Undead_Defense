using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAttackState : SoldierBaseState
{
    public SoldierAttackState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(soldier.AnimationData.AttackParameterHash);
    }
}
