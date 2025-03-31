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
    public override void Update()
    {
        base.Update();
        if (soldier.target == null || Vector3.Distance(soldier.target.position, soldier.transform.position) >= soldier.attackDistance)
        {
            soldierStateMachine.ChangeState(soldierStateMachine.preState);
        }
        if(Vector3.Angle(soldier.transform.forward, soldier.target.position - soldier.transform.position) <= 10f)
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
            soldier.transform.rotation = Quaternion.RotateTowards(soldier.transform.rotation, targetRotation, soldier.rotateSpeed);
        }
    }
}
