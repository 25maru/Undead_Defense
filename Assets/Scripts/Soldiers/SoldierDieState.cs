using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDieState : SoldierBaseState
{
    public SoldierDieState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(soldier.AnimationData.DieParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(soldier.AnimationData.DieParameterHash);
    }
}
