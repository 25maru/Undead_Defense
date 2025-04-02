using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : PlayerBaseState
{
    public PlayerDieState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    // Start is called before the first frame update
    public override void Enter()
    {
        base.Enter();
        player.animator.SetTrigger(playerStateMachine.Player.AnimationData.DieParameterHash);
        playerStateMachine.isDeath = true;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();

    }
}
