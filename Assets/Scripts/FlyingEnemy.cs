using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed;
    public bool chase = false;
    public Transform startingPoint;
    private GameObject player;
    private bool isPlayerNull;

    // Start is called before the first frame update
    private void Start()
    {
        isPlayerNull = player == null;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerNull)
            return;
        if (chase)
        { 
            Chase();
        }
        else
        {
            ReturnStartPoint();
            Flip();
        }
    }

    private void Chase()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, player.transform.position, 
                speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, player.transform.position) <= 0.5f)
        {
            
        }
    }
    
    private void Flip()
    {
        transform.rotation = Quaternion.Euler(0, transform.position.x > player.transform.position.x ? 0 : 180, 0);
    }
    
    private void ReturnStartPoint()
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                startingPoint.position, speed * Time.deltaTime);
        }
}
