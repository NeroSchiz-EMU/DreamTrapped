using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public Healthbar playerHealth;
    private Player player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.changeHealth(+20);
        }
    }
}
