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
    public float defaultMoveSpeed;
    
    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown; 
    public float battleTime;
    public float maxCombatRange;
    public float maxPlayerDetectedRange;
    [HideInInspector] public float lastTimeAttacked; 
    
    [Header("Stunned Info")] 
    public float stunDuration;
    public Vector2 stunDirection; 
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterAttackWindowImage;
    
    public EnemyStateMachine stateMachine {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    public virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }
    
    #region Counter Attack Window
    
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterAttackWindowImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterAttackWindowImage.SetActive(false);
    }
    #endregion
        
    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        
        return false;
    }
    
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, maxPlayerDetectedRange, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
