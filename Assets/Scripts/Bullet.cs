using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public Vector2 direction = new Vector2(0, 0);
    private Vector2 velocity;

    private Player player;
    [SerializeField] private SpriteRenderer sprite;
    private bool facingRightWhenFired;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        Destroy(gameObject, 3);

        if (player.getFacingRight()) facingRightWhenFired = true;
        else if(player.getFacingRight() == false) facingRightWhenFired = false;

        StartCoroutine(Despawn());
    }
    
    void Update()
    {
        if (facingRightWhenFired) velocity = direction * speed;
        else if (!facingRightWhenFired)
        {
            velocity = -direction * speed;
            sprite.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos += velocity * Time.fixedDeltaTime;
        transform.position = pos;
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
