using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}
public abstract class StateMachine
{
    protected IState currentState;
    public bool isDeath = false;
    public virtual void ChangeState(IState state)
    {
        if(isDeath)
            return;

        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}
