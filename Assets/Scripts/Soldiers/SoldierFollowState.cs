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
    void Move()
    {
        //if((soldier.orderTarget.position-soldier.transform.position).sqrMagnitude <= (0.5f * 0.5f))
        //{
        //    StopAnimation(soldier.AnimationData.MoveParameterHash);
        //    StartAnimation(soldier.AnimationData.IdleParameterHash);

        //}
        //else if((soldier.orderTarget.position - soldier.transform.position).sqrMagnitude >= 1f)
        //{
        //    StopAnimation(soldier.AnimationData.IdleParameterHash);
        //    StartAnimation(soldier.AnimationData.MoveParameterHash);
        //}
        soldier.agent.SetDestination(soldier.orderTarget.position);
        Vector3 direction = soldier.agent.desiredVelocity;
        soldier.Controller.Move(direction * Time.deltaTime);
    }
    void Rotate()
    {
        Vector3 direction = soldier.orderTarget.position - soldier.transform.position;
        direction.y = 0f;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            soldier.transform.rotation = Quaternion.RotateTowards(soldier.transform.rotation, targetRotation, soldier.rotateSpeed);
        }
    }
}
