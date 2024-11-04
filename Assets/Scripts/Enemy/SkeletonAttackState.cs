using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        
        enemySkeleton.SetZeroVelocity();

        if (triggerCalled) 
            stateMachine.ChangeState(enemySkeleton.battleState);
            
    }
    
    public override void Exit()
    {
        base.Exit();

        enemySkeleton.lastTimeAttacked = Time.deltaTime;
    }
}
