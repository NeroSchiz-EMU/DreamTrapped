using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    
    
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float coyoteTime = 0.1f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;
	private int airJumpCounter = 0;
	[SerializeField] private int maxAirJumps;
	
	
    [Header("Dash Info")] 
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;
   
    private float xInput;

    private int facingDir = 1;
    private bool facingRight = true;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
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

    }
    
    //****************************************************************************
    //Update is called once per frame
    void Update()   
    {
        Movement();
        CheckInput();
        CollisionChecks();
        FlipController();
        AnimatorControllers();

        //Dashing
        dashTime = dashTime - Time.deltaTime;
        if (Input.GetKeyDown((KeyCode.LeftShift)))
        {
            dashTime = dashDuration;
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
		//else if (!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump"))
		//{
		//	isGrounded = true;
			
		//}

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
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

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

    private void Movement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, rb.velocity.y);
        }
        else
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
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
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
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }

    //****************************************************************************
    //Get functions

    public float getMaxHealth()
    {
        return maxHealth;
    }
    public float getHealth()
    {
        return health;
    }

    //Inventory progression
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

}










