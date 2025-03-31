using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBaseState : IState
{
    protected Soldier soldier;
    protected SoldierStateMachine soldierStateMachine;
    protected Transform model;
    protected Vector3 moveDirection;

    public SoldierBaseState(SoldierStateMachine soldierStateMachine)
    {
        this.soldierStateMachine = soldierStateMachine;
        soldier = soldierStateMachine.soldier;
        model = soldier.model;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void HandleInput()
    {

    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
    }
    protected void StartAnimation(int animationHash)
    {
        soldier.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        soldier.animator.SetBool(animationHash, false);
    }
    protected virtual void Move()
    {
        //겹침 밀어내기 로직 추가필요
        soldier.Controller.Move(moveDirection * Time.deltaTime);
    }
}
