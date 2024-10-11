using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
   protected PlayerStateMachine stateMachine;
   protected Player player;
   
   protected Rigidbody2D rb;
   
   protected float xInput;
   private string animBoolName;
   
   protected float stateTimer;


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
   }
   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
      
      xInput = Input.GetAxisRaw("Horizontal");
      player.animator.SetFloat("yVelocity",rb.velocity.y);
   }
   public virtual void Exit()
   {
      player.animator.SetBool(animBoolName, false);
   }
}
