using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    #region Components

    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX entityFX { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #endregion

    [Header("Knockback Info")] [SerializeField]
    protected Vector2 KnockbackDirection;

    [SerializeField] protected float KnockbackDuration;
    protected bool isKnockedBack = false;


    [Header("Collision Info")] public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    #region Facing properties

    public int facingDirection { get; private set; } = 1;
    public bool isFacingRight { get; private set; } = true;

    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        entityFX = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void Damage()
    {
        entityFX.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
    }


    protected virtual IEnumerator HitKnockback()
    {
        isKnockedBack = true;
        rb.velocity = new Vector2(KnockbackDirection.x * -facingDirection, KnockbackDirection.y);
        yield return new WaitForSeconds(KnockbackDuration);
        isKnockedBack = false;
    }

    #region Velocity

    public void SetZeroVelocity()
    {
        if (isKnockedBack)
            return;

        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {

        if (isKnockedBack)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    #endregion

    #region Collision

    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion

    #region Flip

    public virtual void Flip()
    {
        facingDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if ((_x > 0 && !isFacingRight) || (_x < 0 && isFacingRight))
        {
            Flip();
        }
    }

    #endregion

    public void MakeTransparent(bool _isTransparent)
    {
        if(_isTransparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

}
