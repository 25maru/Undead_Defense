using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMoveState : SoldierBaseState
{
    Vector3 lastPosition;
    float stuckTime = 0f; // 끼인 시간 체크
    float stuckThreshold = 2f; // 2초 이상 끼여 있으면 삭제
    float minMoveDistance = 0.05f;
    public SoldierMoveState(SoldierStateMachine soldierStateMachine) : base(soldierStateMachine)
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
    public override void Update()
    {
        base.Update();
        CheckOrderPosition();
        if(soldier.target == null && soldier.orderTarget == null)
        {
            soldierStateMachine.ChangeState(soldierStateMachine.IdleState);
            return;
        }
        if(soldier.target != null && Vector3.Distance(soldier.target.position,soldier.transform.position) <= soldier.attackDistance /* - 0.1f 여유값 */ )
        {
            soldierStateMachine.ChangeState(soldierStateMachine.AttackState);
            return;
        }
        CheckStuck();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move();
        Rotate();
    }
    protected override void Move()
    {
        soldier.agent.SetDestination(soldier.orderTarget != null ? soldier.orderTarget.position : soldier.target.position);
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
    void CheckOrderPosition()
    {
        if(soldier.orderTarget != null && Vector3.Distance(new Vector3(soldier.orderTarget.position.x,0, soldier.orderTarget.position.z) ,new Vector3(soldier.transform.position.x,0, soldier.transform.position.z)) <= 0.8f)
        {
            soldier.orderTarget = null;
        }
    }
    void CheckStuck()
    {
        float distanceMoved = Vector3.Distance(soldier.transform.position, lastPosition);

        if (distanceMoved < minMoveDistance)
        {
            stuckTime += Time.deltaTime;
        }
        else
        {
            stuckTime = 0f;
        }

        lastPosition = soldier.transform.position;

        if (stuckTime >= stuckThreshold)
        {
            soldier.orderTarget = null;
        }
    }
}
