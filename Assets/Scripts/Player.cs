using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    [SerializeField] ParticleSystem particles;
    [SerializeField] ParticleSystem particlesBoots;
    [SerializeField] ParticleSystem particlesDamage;
    [SerializeField] Animator healthbarAnimator;

    private bool inCutscene;
   
    //Controller movement script------------------------------------------------
    // [SerializeField] private float baseMoveSpeed = 5f; // Base movement speed
    // [SerializeField] private float sensitivity = 1f;   // Sensitivity control for movement speed
    //-------------------------------------------------------------------------
    
    [SerializeField] private float moveSpeed = 5;
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
    private bool isGroundedLeft;
    private bool isGroundedRight;

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
        sprite = GetComponentInChildren<SpriteRenderer>();
        guns = transform.GetComponentsInChildren<Gun>();
         
        //COMMENT IF DEBUGGING
        //transform.position = new Vector3(-34.641f, -5.714f, 0f);
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
                particlesBoots.Play();
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
        isGroundedLeft = Physics2D.Raycast(groundCheckLeft.position,
            Vector2.down, groundCheckDistanceLeft, whatIsGround);
        isGroundedRight = Physics2D.Raycast(groundCheckRight.position,
            Vector2.down, groundCheckDistanceRight, whatIsGround);

        if (isGroundedLeft || isGroundedRight) isGrounded = true;
        else isGrounded = false;
    }

    private void CheckInput()
    {
        if (!inCutscene)
        {
            xInput = Input.GetAxisRaw("Horizontal");

            //Dash ----------------------------------------------------------------------------
            if (Input.GetButtonDown("Dash") && hasDash)
            {
                DashAbility();
            }

            //Melee ----------------------------------------------------------------------------
            if (Input.GetButtonDown("Melee") && hasSword && !isShooting)
            {
                if (isGrounded) isAttacking = true;
                if (!isGrounded) isAirAttacking = true;

                StartCoroutine(Melee());

                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

                foreach(Collider2D enemy in hitEnemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage);
                    }
                }
            }

            //Gun ----------------------------------------------------------------------------
            if (Input.GetButtonDown("Shoot") && hasGun && !isShooting && !isAttacking && !isAirAttacking)
            {
                isShooting = true;
                StartCoroutine(Shoot());

                foreach (Gun gun in guns)
                {
                    gun.Shoot();
                }
            }

            /*if (Input.GetButtonUp("Shoot") && hasGun)
            {
                if (isGrounded)
                    isShooting = false;
            }*/

            //Jumping (dynamic height)
            if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                particles.Play();

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
    
    // Previous Movement Method
    //
    //     if (isAttacking)
    //     {
    //         rb.velocity = new Vector2(0,0);
    //     }
    //     
    //     else if (dashTime > 0)
    //     {
    //         rb.velocity = new Vector2(facingDir * dashSpeed, 0);
    //     }
    //     else if (!inCutscene)
    //     {
    //         rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    //     }
    // }

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

        //If it was damage
        if(h > 0)
        {
            healthbarAnimator.SetTrigger("damaged");
            particlesDamage.Play();
            StartCoroutine(DamageFlash());
        }
    }
    public void resetHealth()
    {
        health = 0;
    }

    IEnumerator DamageFlash()
    {
        sprite.color = new Color(1f, 0.5f, 0.5f, 1f); //Set color to slight red
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1f, 1f, 1f, 1f); //Set color to normal
    }

    IEnumerator Melee()
    {
        yield return new WaitForSeconds(0.4f); //Duration of attack animation
        isAttacking = false;
        isAirAttacking = false;
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(0.2f); //Duration of shoot animation
        isShooting = false;
    }
}
