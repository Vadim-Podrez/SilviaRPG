using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        rb.velocity = new Vector2(0, 0);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update() 
    {
        base.Update();

        if (Mathf.Approximately(xInput, player.facingDirection) && player.IsWallDetected())
            return;
        
        if (xInput != 0)
            stateMachine.ChangeState(player.moveState);
    }

}