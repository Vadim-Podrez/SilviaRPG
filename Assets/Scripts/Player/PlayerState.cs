using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
   protected PlayerStateMachine stateMachine;
   protected Player player;
   
   protected Rigidbody2D rb;
   
   protected float xInput;
   protected float yInput;
   
   private string animBoolName;
   
   protected float stateTimer;
   protected bool triggerCalled;
   
   public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
   {
      this.player = _player;
      this.stateMachine = _stateMachine;
      this.animBoolName = _animBoolName;
   }
   
   public virtual void Enter()
   {
      player.animator.SetBool(animBoolName, true);
      rb = player.rb;
      triggerCalled = false;   
   }
   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
      
      xInput = Input.GetAxisRaw("Horizontal");
      yInput = Input.GetAxisRaw("Vertical");
      player.animator.SetFloat("yVelocity",rb.velocity.y);
   }
   public virtual void Exit()
   {
      player.animator.SetBool(animBoolName, false);
   }

   public virtual void AnimationFinishedTrigger()
   {
      triggerCalled = true;
   }
}
