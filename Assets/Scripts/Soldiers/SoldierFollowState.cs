using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollowState : SoldierBaseState
{
    public SoldierFollowState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
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
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Rotate();
        Move();
    }
    protected override void Move()
    {
        soldier.agent.SetDestination(soldier.orderTarget.position);
        moveDirection = soldier.agent.desiredVelocity;
        base.Move();
    }
    void Rotate()
    {
        Vector3 direction = moveDirection;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            soldier.transform.rotation = Quaternion.RotateTowards(soldier.transform.rotation, targetRotation, soldier.rotateSpeed);
        }
    }
}
