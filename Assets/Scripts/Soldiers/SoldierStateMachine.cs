using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierStateMachine : StateMachine
{
    public Soldier soldier { get; }
    public SoldierIdleState IdleState { get; set; }
    public SoldierHoldState HoldState { get; set; }
    public SoldierMoveState MoveState { get; set; }
    public SoldierAttackState AttackState { get; set; }
    public SoldierDieState DieState { get; set; }

    public SoldierStateMachine(Soldier soldier)
    {
        this.soldier = soldier;


        IdleState = new SoldierIdleState(this);
        HoldState = new SoldierHoldState(this);
        MoveState = new SoldierMoveState(this);
        AttackState = new SoldierAttackState(this);
        DieState = new SoldierDieState(this);
    }
}
