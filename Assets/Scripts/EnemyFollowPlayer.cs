using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float speed;
    public float lineOfSight;
    private Transform playerPos;
    private Player player;
    
    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    void Update()
    {
        //Only follow the player if they're outside of a cutscene
        if(player.getInCutscene() == false)
        {
            float distanceFromPlayer = Vector2.Distance(playerPos.position, transform.position);
            if (distanceFromPlayer < lineOfSight)
            {
                transform.position = Vector2.MoveTowards(this.transform.position,
                             playerPos.position, speed * Time.deltaTime);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lineOfSight); 
    }
    
}