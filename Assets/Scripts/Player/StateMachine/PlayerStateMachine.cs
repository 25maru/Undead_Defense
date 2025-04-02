using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public Transform MainCameraTransform { get; set; }
    public PlayerIdleState IdleState { get; set; }
    public PlayerMoveState MoveState { get; set; }
    public PlayerAttackState AttackState { get; set; }
    public PlayerDieState DieState { get; set; }

    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        MainCameraTransform = Camera.main.transform;

        IdleState = new PlayerIdleState(this);
        MoveState = new PlayerMoveState(this);
        AttackState = new PlayerAttackState(this);
        DieState = new PlayerDieState(this);
    }
}
