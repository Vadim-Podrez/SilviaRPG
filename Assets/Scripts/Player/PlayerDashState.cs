using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }
    
    public override void Enter()
    {
        base.Enter();
        
        player.skill.clone.CreateClone(player.transform, new Vector3(player.facingDirection, 0, 0));
        
        stateTimer = player.dashDuration;
    }
    public override void Exit()
    {
        base.Exit();
        
        player.SetVelocity(0, rb.velocity.y);
    }
    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);
                
        if(stateTimer < 0)
            stateMachine.ChangeState(player.idleState);
    }
    
}
