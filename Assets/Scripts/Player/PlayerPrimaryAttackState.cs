using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerPrimaryAttackState : PlayerState
{

    private int comboCounter;
    
    private float lastTimeAttacked;
    private float comboWindow = 2;
    
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow) 
            comboCounter = 0; 
        
        player.animator.SetInteger("ComboCounter", comboCounter);

        #region Attack Diraction

        float attackDirection = player.facingDirection;

        if (xInput != 0) 
            attackDirection = xInput;
    
        #endregion

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDirection, player.attackMovement[comboCounter].y);
        
        stateTimer = .1f;
    }
    
    public override void Exit()
    {
        base.Exit();
        
        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;   
    }
    
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) 
            player.ZeroVelocity();

        if (triggerCalled) 
            stateMachine.ChangeState(player.idleState);
    }
}
