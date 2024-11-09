using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMoveState : SkeletonGroundedState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName, enemySkeleton)
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
        
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDirection, rb.velocity.y);
        
        
        if (enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundDetected())
        {
            enemySkeleton.Flip();
            
            stateMachine.ChangeState(enemySkeleton.idleState);
            
        }
    }
}
