using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private bool inCutscene;
   
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Jump Info")]
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float coyoteTimeCounter;
    private bool doubleJump;
    private bool isJumping;

    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float jumpBufferCounter;

    [Header("Dash Info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Attack Info")] 
    private bool isAttacking;
    private bool isAirAttacking;
    [SerializeField] protected Transform attackArea;
    public float attackRange;
    public int attackDamage;
    public LayerMask enemyLayers;

    [Header("Shooting Info")]
    Gun[] guns;
    private bool isShooting;
    
    
    private float xInput;
    
    private int facingDir = 1;
    private bool facingRight = true;

    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheckLeft;
    [SerializeField] private float groundCheckDistanceLeft;
    [SerializeField] protected Transform groundCheckRight;
    [SerializeField] private float groundCheckDistanceRight;
    [SerializeField] private LayerMask whatIsGround;
    
    private bool isGrounded;

    [Header("Item Progression")]
    [SerializeField] private bool hasSword;
    [SerializeField] private bool hasBoots;
    [SerializeField] private bool hasGun;
    [SerializeField] private bool hasDash;

    [Header("Health Info")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 0;

    
    //****************************************************************************
    // Start is called before the first frame update
    void Start()
    {
         rb = GetComponent<Rigidbody2D>();
         anim = GetComponentInChildren<Animator>();
         guns = transform.GetComponentsInChildren<Gun>();
         
        //COMMENT IF DEBUGGING
        transform.position = new Vector3(-34.641f, -5.714f, 0f);
    }
    //****************************************************************************

    
    void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();
        FlipController();
        AnimatorControllers();

        //Dashing
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
        
        //Double Jump
        if (isGrounded && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        if (Input.GetButtonDown("Jump") && hasBoots)
        {
            if (isGrounded || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                doubleJump = !doubleJump;
            }
        }
        
        //Jump buffer
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //Coyote Time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

    }

    private void CollisionChecks()
    {
        if (groundCheckLeft || groundCheckRight == true)
            isGrounded = true;
        isGrounded = Physics2D.Raycast(groundCheckLeft.position,
            Vector2.down, groundCheckDistanceLeft, whatIsGround);
        isGrounded = Physics2D.Raycast(groundCheckRight.position,
            Vector2.down, groundCheckDistanceRight, whatIsGround);
    }

    private void CheckInput()
    {
        if (!inCutscene)
        {
            xInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Dash") && hasDash)
            {
                DashAbility();
            }

            if (Input.GetButtonDown("Melee") && hasSword)
            {
                if (!isGrounded == false)
                {
                    isAttacking = true;
                    Collider2D[] hitEnemies = 
                        Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);
                    foreach(Collider2D enemy in hitEnemies)
                    {
                        if (enemy.CompareTag("Enemy"))
                        {
                            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                        }
                    }
                }
                    
                
            }
            if (Input.GetButtonUp("Melee") && hasSword)
            {
                if (isGrounded)
                    isAttacking = false;
            }
            
            if (Input.GetButtonDown("Melee") && hasSword)
            {
                if (isGrounded == false)
                    isAirAttacking = true;
            }
            if (Input.GetButtonUp("Melee") && hasSword)
            {
                if (isGrounded)
                    isAirAttacking = false;
            }
            
            if (Input.GetButtonDown("Shoot") && hasGun)
            {
                if (!isGrounded == false)
                    isShooting = true;
                foreach (Gun gun in guns)
                {
                    gun.Shoot();
                }
            }
            if (Input.GetButtonUp("Shoot") && hasGun)
            {
                if (isGrounded)
                    isShooting = false;
            }
            

            //Jumping (dynamic height)
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                jumpBufferCounter = 0f;
            }

            if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

                coyoteTimeCounter = 0f;
            }
        }
        else if (inCutscene)
        {
            rb.velocity = new Vector2(0, -10);
        }
        
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0 && !isAttacking && !inCutscene)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void Movement()
    {
        if (isAttacking)
        {
            rb.velocity = new Vector2(0,0);
        }
        
        else if (dashTime > 0)
        {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else if (!inCutscene)
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void AnimatorControllers()
    {
          bool isMoving = rb.velocity.x != 0;
                
          anim.SetFloat("yVelocity", rb.velocity.y);
          anim.SetBool("isMoving", isMoving);
          anim.SetBool("isGrounded", isGrounded);
          anim.SetBool("isDashing", dashTime > 0);
          anim.SetBool("isAttacking", isAttacking);
          anim.SetBool("isAirAttacking", isAirAttacking);
          anim.SetBool("isShooting", isShooting);
    }

    private void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        anim.transform.Rotate(0f, 180f, 0f);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckLeft.position, 
            new Vector3(groundCheckLeft.position.x, groundCheckLeft.position.y - groundCheckDistanceLeft));
        Gizmos.DrawLine(groundCheckRight.position, 
            new Vector3(groundCheckRight.position.x, groundCheckRight.position.y - groundCheckDistanceRight));
       
    }

     // private void OnTriggerEnter2D(Collider2D collision)
     // {
     //     if (collision.tag == "Hurts")
     //     {
     //         Debug.Log("Entered hurt object, starting health restoration");
     //         StartCoroutine(TakeDamage());
     //         IEnumerator TakeDamage()
     //         {
     //             Debug.Log("Initial health: " + health);
     //             health += 5;
     //             Debug.Log("Health after restore: " + health);
     //             yield return new WaitForSeconds(0.5f);
     //         }
     //         Debug.Log("Coroutine launched");
     //     }
     // }

     


    //****************************************************************************
    //Get functions

    public bool getFacingRight()
    {
        return facingRight;
    }

    public float getMaxHealth()
    {
        return maxHealth;
    }
    public float getHealth()
    {
        return health;
    }

    //Inventory progression - accessed by UI
    public bool getHasSword()
    {
        return hasSword;
    }
    public bool getHasBoots()
    {
        return hasBoots;
    }
    public bool getHasGun()
    {
        return hasGun;
    }
    public bool getHasDash()
    {
        return hasDash;
    }

    public bool getInCutscene()
    {
        return inCutscene;
    }

    //****************************************************************************
    //Set functions

    //Set by mannequin item collection
    public void setHasSword(bool sword)
    {
        hasSword = sword;
    }
    public void setHasBoots(bool boots)
    {
        hasBoots = boots;
    }
    public void setHasGun(bool gun)
    {
        hasGun = gun;
    }
    public void setHasDash(bool dash)
    {
        hasDash = dash;
    }

    public void setInCutscene(bool i)
    {
        inCutscene = i;
    }

    //can take Pos or Neg to take health or heal health respectively
    //our opposite health system is so confusing in code on god
    public void changeHealth(float h)
    {
        health = Mathf.Clamp(health + h, 0, maxHealth); //limits health to 0
        //Debug.Log(health);
    }
    public void resetHealth()
    {
        health = 0;
    }

}




// try this for ground check option
// --------------------------------
// protected bool isGrounded;
// public float groundCheckDistance;
// public LayerMask whatIsGround;








