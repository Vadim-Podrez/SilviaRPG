using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack Movement")] 
    public Vector2[] attackMovement;
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

    [Header("Collision Info")] 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDirection { get; private set; } = 1;
    private bool facingRight = true;
    
    #region Components
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    
    #endregion

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }  
    public PlayerAirState airState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; } 
    public PlayerWallJumpState wallJump{ get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    
    #endregion

    public void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
    }

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        stateMachine.Initialize(idleState);
    }
    
    private void Update()
    {
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0)
        {
            dashUsageTimer = dashCooldown;
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);
        }

    }

    #region Velocity
    public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    #region Collision
    public bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }
    #endregion

    #region PlayerFlip
    public void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180,0);
    }
    
    public void FlipController( float _x)
    {
        if (_x > 0 && !facingRight) 
            Flip();
        else if(_x < 0 && facingRight)
            Flip();
    }
    #endregion

}
