using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilBrawler : Entity
{
    
    [Header("Move Info")] 
    [SerializeField] private float moveSpeed;
    
    private bool isAttacking;
    private RaycastHit2D isPlayerDetected;
    
    protected override void Start()
    {
        base.Start();
        
    }
    
    protected override void Update()
    {
        base.Update();
        
        
        Movement();
        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 2.5f * facingDir, rb.velocity.y);
                Debug.Log("I see the player!");
                isAttacking = false;
            }
            else
            {
                Debug.Log("ATTACK!! " + isPlayerDetected.collider.gameObject.name);
                isAttacking = true;
            }
        }
        

        if (!isGrounded || isWallDetected)
            Flip();
    }
    

    private void Movement()
    {
        if(!isAttacking)
            rb.velocity = new Vector2(moveSpeed * facingDir, rb.velocity.y);
    }

    protected override void CollisionChecks()
    {
       base.CollisionChecks();

       isPlayerDetected = Physics2D.Raycast(playerCheck.position,
           Vector2.right, playerCheckDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // Gizmos.color = Color.blue;
        // Gizmos.DrawLine(playerCheck.position,
        //     new Vector3(transform.position.x + playerCheckDistance * facingDir, transform.position.y));
    }
    
}












