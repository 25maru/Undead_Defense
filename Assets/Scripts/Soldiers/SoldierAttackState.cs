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
        soldier.agent.ResetPath();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(soldier.AnimationData.AttackParameterHash);
    }
    public override void Update()
    {
        base.Update();
        if (soldier.target == null || Vector3.Distance(soldier.target.position, soldier.transform.position) >= soldier.attackDistance)
        {
            soldierStateMachine.ChangeState(soldierStateMachine.preState);
        }
        if(Vector3.Angle(soldier.model.forward, soldier.target.position - soldier.transform.position) <= 10f)
        {
            StartAnimation(soldier.AnimationData.AttackParameterHash);
        }
        else
        {
            StopAnimation(soldier.AnimationData.AttackParameterHash);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Rotate();
    }
    void Rotate()
    {
        Vector3 direction = soldier.target.position - soldier.transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            soldier.model.localRotation = Quaternion.RotateTowards(soldier.model.localRotation, targetRotation, soldier.rotateSpeed);
        }
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
