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
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    
    protected bool isGrounded;
   
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    
    protected virtual void Update()
    {
        CollisionChecks();
    }
    
    protected virtual void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position,
            Vector2.down, groundCheckDistance, whatIsGround);
    }
    
    protected virtual void Flip()
    {
        facingDir = -facingDir;
        facingRight = !facingRight;
        anim.transform.Rotate(0f, 180f, 0f);
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