using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName, enemySkeleton)
    {
    }

    public override void Enter()
    { 
        base.Enter();

        stateTimer = enemySkeleton.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }
    
    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) 
            stateMachine.ChangeState(enemySkeleton.moveState);
        
    }
}
