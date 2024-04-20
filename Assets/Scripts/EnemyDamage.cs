using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public int damage;
    public Healthbar playerHealth;
    private Player player;

    private bool coroutineStarted;
    private bool insideCloud;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    //Enemies
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.changeHealth(+10);
        }
    }

    //Clouds
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insideCloud = true;
            player.changeHealth(+5);
            StartCoroutine(CloudDamage());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            insideCloud = false;
        }
    }

    IEnumerator CloudDamage()
    {
        while (insideCloud)
        {
            yield return new WaitForSeconds(1f);
            if (!insideCloud) break; //If player has exited cloud since timer started
            player.changeHealth(+2);
        }
    }
}
