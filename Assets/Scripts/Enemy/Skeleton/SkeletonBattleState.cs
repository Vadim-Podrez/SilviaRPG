using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemySkeleton;
    private int moveDirection;


    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton enemySkeleton) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = enemySkeleton;
    }
    public override void Enter()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (enemySkeleton.IsPlayerDetected())
        {
            stateTimer = enemySkeleton.battleTime;
            
            if (enemySkeleton.IsPlayerDetected().distance < enemySkeleton.attackDistance)
            {
                if (CanAttack()) 
                    stateMachine.ChangeState(enemySkeleton.attackState);
            }   
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemySkeleton.transform.position) > enemySkeleton.maxCombatRange)
            {
                stateMachine.ChangeState(enemySkeleton.idleState);
            }
        }

        if (player.position.x > enemySkeleton.transform.position.x)
            moveDirection = 1;
        else if (player.position.x < enemySkeleton.transform.position.x) 
            moveDirection = -1;
        
        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * moveDirection, rb.velocity.y);
    }
    
    public override void Exit()
    {
        base.Exit();
        
        
    }

    private bool CanAttack()
    {
        if (Time.time >= enemySkeleton.lastTimeAttacked + enemySkeleton.attackCooldown)
        {
            enemySkeleton.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
}
