using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EnemyStateMachine
{ 
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    
    private string animBoolName;
    
    protected bool triggerCalled;
    protected float stateTimer;
    
    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    
    public virtual void Enter()
    {
        triggerCalled = false; 
        rb = enemyBase.rb;
        enemyBase.animator.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void Exit()
    {
        enemyBase.animator.SetBool(animBoolName, false);
    }
    
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
    
}
