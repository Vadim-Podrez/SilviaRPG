using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemySkeleton;
    protected Transform player;
    
    public SkeletonGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public override void Exit()
    {
        base.Exit();
    }   
    public override void Update()
    {
        base.Update();
        
        
        if (enemySkeleton.IsPlayerDetected() || Vector2.Distance(enemySkeleton.transform.position, player.position) < 2) 
            stateMachine.ChangeState(enemySkeleton.battleState);
    }
    
}
