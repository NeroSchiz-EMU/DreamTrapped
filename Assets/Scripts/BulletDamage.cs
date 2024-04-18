using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    public int bulletDamage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "GunBlock")
        {
            other.gameObject.GetComponent<GunBlockHealth>().damageGunBlock(bulletDamage);
            Destroy(gameObject);
        }else if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().damageEnemy(bulletDamage);
            Destroy(gameObject);
        }
    }
}
