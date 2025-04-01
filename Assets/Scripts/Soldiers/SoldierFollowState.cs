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
        
        Move();
        Rotate();
    }
    protected override void Move()
    {
        soldier.agent.SetDestination(soldier.orderTarget.position);
        moveDirection = soldier.agent.desiredVelocity.normalized;
        soldier.agent.nextPosition = soldier.transform.position;
        soldier.Controller.Move(((moveDirection * soldier.moveSpeed) + (Vector3.down * soldier.downSpeed)) * Time.deltaTime);
    }
    void Rotate()
    {
        Vector3 direction = moveDirection;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            soldier.model.localRotation = Quaternion.RotateTowards(soldier.model.localRotation, targetRotation, soldier.rotateSpeed);
        }
    }
}
