using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SkeletonStunnedState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    
    public SkeletonStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        
        enemySkeleton.entityFX.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemySkeleton.stunDuration;
        
        rb.velocity = new Vector2(-enemySkeleton.facingDirection * enemySkeleton.stunDirection.x, enemySkeleton.stunDirection.y);
        
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0) 
            stateMachine.ChangeState(enemySkeleton.idleState);
    }

    public override void Exit()
    {   
        base.Exit();
        
        enemySkeleton.entityFX.Invoke("CancelRedColorBlink", 0);
    }
}
