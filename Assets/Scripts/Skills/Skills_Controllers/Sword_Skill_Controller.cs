using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2D;
    private Player player;
    
    private bool canRotate = true;
    private bool isReturning;   
    private float returnSpeed = 20;
    private float freezeTimeDuration;
    
    [Header ("Pierce info")]
    private float pierceAmount;
    
    [Header("Bounce info")]
    private float bounceSpeed;
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;    
    
    [Header ("Spin info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool isSpinning;
    private bool wasStopped;
    private float hitTimer;
    private float hitCooldown;
    private float spinDirection;
    
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
    public void SetupSword(Vector2 _direction, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        
        rb.velocity = _direction;
        rb.gravityScale = _gravityScale;
        
        if (pierceAmount <= 0) 
            animator.SetBool("Rotation", true);
        
        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        
        Invoke("DestroyMe", 10);
    }
    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;     
    }
    public void SetupBounce(bool _isBouncing, int _bounceAmount, float _bounceSpeed)
    {
        bounceSpeed = _bounceSpeed;
        isBouncing = _isBouncing;
        bounceAmount = _bounceAmount;
        
        enemyTarget = new List<Transform>();
    }
    public void SetupPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }
    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }
    public void Update()
    {
        if (canRotate) 
            transform.right = rb.velocity;
        
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1) 
                player.CatchTheSword();
        }
        
        BounceLogic();
        SpinLogic();
        
    }
    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
            {
                StopWhenSpinning();
            }

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;
                
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                
                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }
            }
            
            hitTimer -= Time.deltaTime;

            if (hitTimer < 0)
            {
                hitTimer = hitCooldown;
                
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        SwordSkillDamage(hit.GetComponent<Enemy>());
                }
            }
        }
    }
    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }
    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                
                targetIndex++;
                bounceAmount--;
                
                if (bounceAmount <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }
                
                if (targetIndex >= enemyTarget.Count) 
                    targetIndex = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }
        
        SetupTargetForBounce(collision);
        StuckInfo(collision);
    }
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }
    private void SetupTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null) 
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }
    private void StuckInfo(Collider2D collision)
    {
        if (pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;     
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        circleCollider2D.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;
        
        animator.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
