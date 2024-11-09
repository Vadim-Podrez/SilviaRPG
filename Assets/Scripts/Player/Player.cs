using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")] 
    public Vector2[] attackMovement;

    public float counterAttackDuration = .2f;
    
    public bool isBusy {get; private set;}
    
    [Header("Move Info")] 
    public float moveSpeed = 12f;
    public float jumpForce;
    
    [Header("Dash Info")] 
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer; 
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }
    
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }  
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; } 
    public PlayerWallJumpState wallJump{ get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    public PlayerCounterAttackState counterAttack { get; private set; }
    
    #endregion

    protected override void Awake()
    {
        base.Awake();
        
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);
    } 
        
    
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
        CheckForDashInput(); 
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        
        yield return new WaitForSeconds(_seconds);
        
        isBusy = false;
    }
    
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishedTrigger();

    private void CheckForDashInput()
    {
        if (IsWallDetected())
            return;
        
        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.F) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);
        }
    }
}
