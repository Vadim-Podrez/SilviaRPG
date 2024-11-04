using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    
    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    
    
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown; 
    public float battleTime;
    public float maxCombatRange;
    public float maxPlayerDetectedRange;
    [HideInInspector] public float lastTimeAttacked; 
    
    public EnemyStateMachine stateMachine {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
        
    }
    
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, maxPlayerDetectedRange, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}