using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator anim;
    
    protected int facingDir = 1;
    protected bool facingRight = true;

    [Header("Collision Info")] 
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [Space]
    [Header("End of Enitiy")]
    [Space]
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected LayerMask whatIsPlayer;
    
    protected bool isGrounded;
    protected bool isWallDetected;
    protected bool IsPlayerDetected;
   
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        if (wallCheck == null)
            wallCheck = transform;
        
        if (playerCheck == null) 
            playerCheck = transform;
    }
    
    protected virtual void Update()
    {
        CollisionChecks();
    }
    
    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(groundCheck.position,
            Vector2.down, groundCheckDistance, whatIsGround);
        
        isWallDetected = Physics2D.Raycast(wallCheck.position,
            Vector2.right, wallCheckDistance * facingDir, whatIsGround);
        
        IsPlayerDetected = Physics2D.Raycast(playerCheck.position,
            Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }
    
    protected virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        anim.transform.Rotate(0f, 180f, 0f);
    }
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, 
            new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,
            new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position,
            new Vector3(playerCheck.position.x + playerCheckDistance * facingDir, playerCheck.position.y));
    }
}


// put the code below in
// the class for the pencil and pillow enemies

// protected override void Start()
// {
//     base.Start();
//         *insert code*
// }

// then also for update

// protected override void Update()
// {
//     base.Update();
//         *insert code*
// } 