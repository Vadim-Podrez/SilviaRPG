using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    } 
    
    

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    
        if (Input.GetKeyDown(KeyCode.Mouse0)) 
            stateMachine.ChangeState(player.primaryAttackState);
        
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        if (player.IsWallDetected() && !player.IsGroundDetected())
            stateMachine.ChangeState(player.wallSlide);
    }
}
