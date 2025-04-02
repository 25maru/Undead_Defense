using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoldierStateMachine : StateMachine
{
    public Soldier soldier { get; }
    public SoldierIdleState IdleState { get; set; }
    public SoldierMoveState MoveState { get; set; }
    public SoldierAttackState AttackState { get; set; }
    public SoldierDieState DieState { get; set; }
    public SoldierFollowState FollowState { get; set; }
    public SoldierHoldState HoldState { get; set; }

    public IState preState;

    public SoldierStateMachine(Soldier soldier)
    {
        this.soldier = soldier;

        IdleState = new SoldierIdleState(this);
        MoveState = new SoldierMoveState(this);
        AttackState = new SoldierAttackState(this);
        DieState = new SoldierDieState(this);
        FollowState = new SoldierFollowState(this);
        HoldState = new SoldierHoldState(this);
    }

    public override void ChangeState(IState state)
    {
        preState = currentState;
        base.ChangeState(state);
    }
}
