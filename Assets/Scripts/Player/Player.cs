using System;
using System.Collections;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Player : Entity
{
    
    [Header("Attack Details")] 
    public Vector2[] attackMovement;
    public float swordReturnImpact;

    public float counterAttackDuration = .2f;
    public bool isBusy {get; private set;}
    
    [Header("Move Info")] 
    public float moveSpeed = 12f;
    public float jumpForce;
    
    [Header("Dash Info")] 
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection { get; private set; }
    
    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }
    
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
    public PlayerAimState aimSwordState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }
    
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
        aimSwordState = new PlayerAimState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
        
    }   

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;
        
        stateMachine.Initialize(idleState);
    }

    public void AssignNewSword(GameObject _sword)
    {
        sword = _sword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
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

        if (Input.GetKeyDown(KeyCode.F) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDirection = Input.GetAxisRaw("Horizontal");

            if (dashDirection == 0)
                dashDirection = facingDirection;

            stateMachine.ChangeState(dashState);
        }
    }
}
